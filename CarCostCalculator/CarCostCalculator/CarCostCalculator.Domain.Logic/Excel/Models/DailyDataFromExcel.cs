using CarCostCalculator.Domain.Logic.Excel.Models.Base;

namespace CarCostCalculator.Domain.Logic.Excel.Models;

public class DailyDataFromExcel : BaseFromExcel
{
    #region Public Properties

    public required DateOnly Date { get; set; }

    public Dictionary<string, decimal> Expenses { get; set; } = [];

    public decimal KilometersDriven { get; set; }

    #endregion
}