using CarCostCalculator.Common;
using CarCostCalculator.Domain.Logic.Excel.Models;
using CarCostCalculator.Domain.Model.Common;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;

namespace CarCostCalculator.Domain.Logic.Excel;

public partial class ExcelHandler(ILogger<ExcelHandler> logger, ActivitySource activitySource)
    : IExcelHandler
{
    #region Private Members

    private readonly ActivitySource _activitySource = activitySource;
    private readonly ILogger<ExcelHandler> _logger = logger;
    private SharedStringItem[] _sharedStrings = [];

    #endregion

    #region Public Methods

    public List<CarExpenseFromExcel> GetCarExpenseData(ExcelSheet sheet, UserLogs userLogs)
    {
        var result = new List<CarExpenseFromExcel>();
        var maxRow = sheet.GetMaxRow();

        for (var row = ExcelImportType.CarExpenseImportMonth.GetStartRow(); row <= maxRow; row++)
        {
            var excelRow = $"Row: {row} Sheet:\"{sheet.Name}\"";

            var carExpense = new CarExpenseFromExcel
            {
                ExcelRow = excelRow,
                CarInsurance = StringToDecimal(sheet["C2", row]),
                Date = StringToDateOnly(sheet["B2", row], userLogs),
                Fuel = StringToDecimal(sheet["I2", row]),
                Inspection = StringToDecimal(sheet["E2", row]),
                KilometersDriven = StringToDecimal(sheet["L2", row]),
                OAMTC = StringToDecimal(sheet["G2", row]),
                Other = StringToDecimal(sheet["J2", row]),
                Registration = StringToDecimal(sheet["D2", row]),
                Service = StringToDecimal(sheet["F2", row]),
                Vignette = StringToDecimal(sheet["H2", row])
            };

            result.Add(carExpense);
        }

        return result;
    }

    public ICollection<ExcelSheet> LoadData(FileCommandItem file, UserLogs userLogs)
    {
        using var activity = _activitySource.StartActivity("Importing Excel sheets");
        activity?.AddTag("File.Name", file.FileName);

        ICollection<ExcelSheet> excelSheets = [];
        using var doc = SpreadsheetDocument.Open(file.OpenReadStream(), false);

        var workbookPart = doc.WorkbookPart
            ?? throw new InvalidDataException("The provided file is not a valid Excel file.");

        var shareStringPart = workbookPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault()
            ?? throw new InvalidDataException("Excel file is empty.");

        _sharedStrings = [.. shareStringPart.SharedStringTable!.Elements<SharedStringItem>()];

        var sheetNamesDict = new ReadOnlyDictionary<string, string?>(doc.WorkbookPart.Workbook!
            .Descendants<Sheet>()
            .Where(s => s.Id != null)
            .ToDictionary(s => s.Id!.ToString()!, s => s.Name?.ToString()));

        using var userLogsActivity = _activitySource.StartActivity("Validate and Map sheets");

        foreach (var worksheet in workbookPart.WorksheetParts)
        {
            var sheetName = $"{file.FileName} - {sheetNamesDict.GetValueOrDefault(workbookPart.GetIdOfPart(worksheet))}";

            var cells = worksheet.Worksheet!
                .Descendants<Cell>()
                .Where(c => c.CellReference != null)
                .ToDictionary(c => c.CellReference!.ToString()!, GetCellValue);

            ExcelImportType employeeImportType;
            string? faultyKey = null;
            string? faultyValue = null;

            if (ValidateSheet(ExcelSheetTemplates.MonthlyCarExpenseTemplate.Template, cells, ref faultyKey, ref faultyValue))
            {
                employeeImportType = ExcelImportType.CarExpenseImportMonth;
            }
            else if (ValidateSheet(ExcelSheetTemplates.AnnualCarExpenseTemplate.Template, cells, ref faultyKey, ref faultyValue))
            {
                employeeImportType = ExcelImportType.CarExpenseImportYear;
                userLogs.LogInformation(_logger, $"Sheet \"{sheetName}\" conforms to the yearly car expense template. It will not be imported.");
                continue;
            }
            else if (faultyKey == null)
            {
                userLogs.LogError(_logger, $"Sheet \"{sheetName}\" invalid. It did not conform to any template.");
                continue;
            }
            else
            {
                userLogs.LogError(_logger, $"Sheet \"{sheetName}\" invalid. The following cell did not conform to any template: {faultyKey} = {faultyValue}");
                continue;
            }

            excelSheets.Add(new(employeeImportType, cells, sheetName));
        }

        return excelSheets;
    }

    #endregion

    #region Private Methods

    private static decimal StringToDecimal(string? str)
    {
        if (string.IsNullOrWhiteSpace(str))
        {
            return 0m;
        }

        if (decimal.TryParse(str, NumberStyles.Number | NumberStyles.AllowExponent, CultureInfo.CreateSpecificCulture("en"), out var decimalValue))
        {
            return decimalValue;
        }

        return 0m;
    }

    private static bool ValidateSheet(IReadOnlyDictionary<string, string> template, Dictionary<string, string?> sheet, ref string? faultyKey, ref string? faultyValue)
    {
        var first = true;
        foreach (var templateCell in template)
        {
            if (!sheet.TryGetValue(templateCell.Key, out var sheetValue) || sheetValue?.Contains(templateCell.Value, StringComparison.OrdinalIgnoreCase) != true)
            {
                if (!first)
                {
                    faultyKey = templateCell.Key;
                    faultyValue = templateCell.Value;
                }

                return false;
            }

            first = false;
        }

        return true;
    }

    /// <summary>
    /// If the content of the cell is stored as a shared string, get the text of the cell from the SharedStringTablePart and return it.
    /// Otherwise, return the string value of the cell.
    /// </summary>
    /// <param name="cell"><see cref="Cell"/></param>
    /// <returns>The text of the cell</returns>
    private string? GetCellValue(Cell cell)
    {
        string? result;

        if (cell.DataType is not null && cell.DataType.Value == CellValues.SharedString && int.TryParse(cell.CellValue?.Text, out var index))
        {
            result = _sharedStrings[index].InnerText;
        }
        else
        {
            result = cell.CellValue?.Text;
        }

        if (string.IsNullOrWhiteSpace(result) || result.Equals("null", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }
        else
        {
            return result;
        }
    }

    private DateOnly StringToDateOnly(string? str, UserLogs userLogs)
    {
        if (string.IsNullOrWhiteSpace(str))
        {
            userLogs.LogError(_logger, "Date string is null or empty.");
        }

        if (DateOnly.TryParse(str, CultureInfo.CreateSpecificCulture("en"), DateTimeStyles.None, out var dateValue))
        {
            return dateValue;
        }

        userLogs.LogError(_logger, $"Failed to parse date string: \"{str}\". Returning default value.");
        return dateValue;
    }

    #endregion
}