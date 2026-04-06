namespace CarCostCalculator.Domain.Logic.Excel.Models;

public class SheetDataFromExcel
{
    #region Public Properties

    public List<DailyDataFromExcel> DailyData { get; set; } = [];

    public List<string> ExpenseCategories { get; set; } = [];

    public required string SheetName { get; set; }

    #endregion
}