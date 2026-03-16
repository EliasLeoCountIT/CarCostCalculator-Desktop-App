using System.Collections.ObjectModel;

namespace CarCostCalculator.Domain.Logic.Excel.Models;

public class ExcelSheetTemplate(ExcelImportType importType, ReadOnlyDictionary<string, string> template)
{
    #region Public Properties

    public ExcelImportType ImportType => importType;

    public IReadOnlyDictionary<string, string> Template => template;

    #endregion
}