using CarCostCalculator.Common;
using CarCostCalculator.Data.Contract;
using CarCostCalculator.Domain.Logic.Excel;
using CarCostCalculator.Domain.Logic.Excel.Models;
using CarCostCalculator.Domain.Model.CarExpense;
using CarCostCalculator.Domain.Model.Common;
using CarCostCalculator.Domain.Model.ExpenseCategory;
using CarCostCalculator.Domain.Model.Kilometers;
using Microsoft.AspNetCore.Mvc;

namespace CarCostCalculator.Web.API.Controller;

[Route("api/[controller]")]
[ApiController]
public class CarExpenseController(ICarExpenseRepository carExpenseRepo,
                                  IKilometersRepository kilometersRepo,
                                  IExpenseCategoryRepository expenseCategoryRepo,
                                  ILogger<CarExpenseController> logger,
                                  IExcelHandler excelHandler)
    : ControllerBase
{
    #region Private Members

    private const int CATEGORIES_ROW = 2;

    private readonly ICarExpenseRepository _carExpenseRepo = carExpenseRepo;
    private readonly IExcelHandler _excelHandler = excelHandler;
    private readonly IExpenseCategoryRepository _expenseCategoryRepo = expenseCategoryRepo;
    private readonly IKilometersRepository _kilometersRepo = kilometersRepo;
    private readonly ILogger<CarExpenseController> _logger = logger;

    #endregion

    #region Public Methods

    // DELETE api/CarExpense/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            await _carExpenseRepo.Delete(id, cancellationToken);
            return Ok("Successfully deleted");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    // GET: api/CarExpense
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CarExpenseChangeable>>> Get(
        CancellationToken cancellationToken = default)
    {
        var CarExpenses = await _carExpenseRepo.LoadAll(cancellationToken);
        return Ok(CarExpenses);
    }

    // GET api/CarExpense/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CarExpenseChangeable>> Get(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            var CarExpense = await _carExpenseRepo.LoadByPrimaryKey(id, cancellationToken);
            return Ok(CarExpense);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    // GET api/CarExpense/date/2025-12-06
    [HttpGet("date/{date}")]
    public async Task<ActionResult<CarExpenseChangeable>> GetByDate(DateOnly date, CancellationToken cancellationToken = default)
    {
        try
        {
            var carExpense = await _carExpenseRepo.LoadByDate(date, cancellationToken);
            return Ok(carExpense);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    // GET api/CarExpense/month/2025/12
    [HttpGet("month/{year}/{month}")]
    public async Task<ActionResult<IEnumerable<CarExpenseChangeable>>> GetByMonth(int year, int month, CancellationToken cancellationToken = default)
    {
        if (month < 1 || month > 12)
        {
            return BadRequest("Month must be between 1 and 12");
        }

        var carExpenses = await _carExpenseRepo.GetByMonth(year, month, cancellationToken);
        return Ok(carExpenses);
    }

    // GET api/CarExpense/range?startDate=2025-01-01&endDate=2025-03-31
    [HttpGet("range")]
    public async Task<ActionResult<IEnumerable<CarExpenseChangeable>>> GetByTimeRange(
        [FromQuery] DateOnly startDate,
        [FromQuery] DateOnly endDate,
        CancellationToken cancellationToken = default)
    {
        if (startDate > endDate)
        {
            return BadRequest("Start date must be before or equal to end date");
        }

        var carExpenses = await _carExpenseRepo.GetByTimeRange(startDate, endDate, cancellationToken);
        return Ok(carExpenses);
    }

    // GET api/CarExpense/year/2025
    [HttpGet("year/{year}")]
    public async Task<ActionResult<IEnumerable<CarExpenseChangeable>>> GetByYear(int year, CancellationToken cancellationToken = default)
    {
        var carExpenses = await _carExpenseRepo.GetByYear(year, cancellationToken);
        return Ok(carExpenses);
    }

    // POST api/CarExpense/ExcelFileImport
    [HttpPost("ExcelFileImport")]
    public async Task<ActionResult> ImportFromExcel(
        IFormFile file,
        CancellationToken cancellationToken = default)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded");
        }

        if (!file.FileName.EndsWith(".xlsx") && !file.FileName.EndsWith(".xls"))
        {
            return BadRequest("Only Excel files (.xlsx, .xls) are supported");
        }

        var userLogs = new UserLogs();

        // Step 1: Convert IFormFile to FileCommandItem
        var fileCommandItem = new FileCommandItem(
            openReadStream: () => file.OpenReadStream(),
            fileName: file.FileName,
            contentType: file.ContentType,
            length: file.Length,
            contentDisposition: file.ContentDisposition
        );

        // Step 2: Load and validate Excel sheets
        var excelSheets = _excelHandler.LoadData(fileCommandItem, userLogs);

        if (excelSheets.Count == 0)
        {
            return BadRequest(new
            {
                Message = "No valid sheets found in the Excel file",
                userLogs.Errors,
                userLogs.Warnings,
                userLogs.Information
            });
        }

        var allSheetData = new List<SheetDataFromExcel>();

        // Step 3: Extract car expenses from each valid sheet
        foreach (var sheet in excelSheets)
        {
            var germanCategories = new[]
           {
                sheet["C", CATEGORIES_ROW],
                sheet["D", CATEGORIES_ROW],
                sheet["E", CATEGORIES_ROW],
                sheet["F", CATEGORIES_ROW],
                sheet["G", CATEGORIES_ROW],
                sheet["H", CATEGORIES_ROW],
                sheet["I", CATEGORIES_ROW],
                sheet["J", CATEGORIES_ROW],
            };

            allSheetData.Add(new SheetDataFromExcel
            {
                SheetName = sheet.Name,
                ExpenseCategories = germanCategories.ToList()!,
                DailyData = _excelHandler.GetCarExpenseData(sheet, userLogs)
            });
        }

        var allDailyDataFromExcel = allSheetData.SelectMany(s => s.DailyData).ToList();

        // import expense categories first
        var expenseCategoriesToImport = allSheetData[0]
            .ExpenseCategories
            .Select(ec => new ExpenseCategoryAddable
            {
                Name = ec,
            });

        var importedExpenseCategories = await _expenseCategoryRepo.CreateMany(expenseCategoriesToImport, cancellationToken);
        var expenseCategoriesCount = importedExpenseCategories.Count();

        if (expenseCategoriesCount != expenseCategoriesToImport.Count())
        {
            userLogs.LogWarning(_logger, "Not all expense categories from Excel file were imported");
        }

        var expenseCategoriesInDb = await _expenseCategoryRepo.LoadAll(cancellationToken);
        var categoryNameToIdMap = expenseCategoriesInDb.ToDictionary(
            ec => ec.Name,
            ec => ec.Id
        );

        // import kilometers
        var kilometersDrivenToImport = allDailyDataFromExcel.Where(x => x.KilometersDriven > 0)
            .Select(x =>
                new KilometersAddable
                {
                    KilometersDriven = x.KilometersDriven,
                    Date = x.Date
                })
            .OrderBy(x => x.Date);

        var importedKilometers = await _kilometersRepo.CreateMany(kilometersDrivenToImport, cancellationToken);
        var importedKilometersCount = importedKilometers.Count();

        if (importedKilometersCount != kilometersDrivenToImport.Count())
        {
            userLogs.LogWarning(_logger, "Not all kilometers from Excel file were imported");
        }

        // import car expenses
        var carExpensesToImport = allDailyDataFromExcel.Where(x => x.Expenses.Count > 0)
            .SelectMany(x =>
                x.Expenses.Select(e =>
                {
                    // Look up the category ID from the name
                    if (!categoryNameToIdMap.TryGetValue(e.Key, out var categoryId))
                    {
                        throw new InvalidOperationException($"Expense category '{e.Key}' not found in imported categories.");
                    }

                    return new CarExpenseAddable
                    {
                        Date = x.Date,
                        Amount = e.Value,
                        ExpenseCategoryId = categoryId,
                        Notes = string.Empty
                    };
                }))
            .OrderBy(x => x.Date);

        var importedCarExpenses = await _carExpenseRepo.CreateMany(carExpensesToImport, cancellationToken);
        var importedExpenseCount = importedCarExpenses.Count();

        if (importedExpenseCount != carExpensesToImport.Count())
        {
            userLogs.LogWarning(_logger, "Not all car expenses from Excel file were imported");
        }

        return Ok(new
        {
            Message = $"File imported successfully. {importedExpenseCount} expense(s) and {importedKilometersCount} kilometer entries imported.",
            ImportedKilometerEntriesCount = importedKilometersCount,
            ImportedCarExpenseEntriesCount = importedExpenseCount,
            ImportedExpenseCategories = expenseCategoriesCount,
            TotalRows = allDailyDataFromExcel.Count,
            userLogs.Errors,
            userLogs.Warnings,
            userLogs.Information
        });
    }

    // POST api/CarExpense
    [HttpPost]
    public async Task<ActionResult<CarExpenseChangeable>> Post(
        [FromBody] CarExpenseAddable CarExpenseAddable,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var CarExpense = await _carExpenseRepo.Create(CarExpenseAddable, cancellationToken);
            return CreatedAtAction(nameof(Get), new { id = CarExpense.Id }, CarExpense);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    // PUT api/CarExpense/5
    [HttpPut("{id}")]
    public async Task<ActionResult<CarExpenseChangeable>> Put(
        long id,
        [FromBody] CarExpenseChangeable CarExpenseChangeable,
        CancellationToken cancellationToken = default)
    {
        if (id != CarExpenseChangeable.Id)
        {
            return BadRequest("Id mismatch");
        }

        try
        {
            var updatedCarExpense = await _carExpenseRepo.Update(CarExpenseChangeable, cancellationToken);
            return Ok(updatedCarExpense);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    #endregion
}