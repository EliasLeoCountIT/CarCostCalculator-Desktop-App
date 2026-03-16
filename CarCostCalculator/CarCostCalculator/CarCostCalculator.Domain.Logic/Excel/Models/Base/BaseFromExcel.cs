namespace CarCostCalculator.Domain.Logic.Excel.Models.Base;

public abstract class BaseFromExcel
{
    #region Public Properties

    public required string ExcelRow { get; set; }

    #endregion
}