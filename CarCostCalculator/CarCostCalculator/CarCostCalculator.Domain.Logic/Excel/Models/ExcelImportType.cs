namespace CarCostCalculator.Domain.Logic.Excel.Models;

public enum ExcelImportType
{
    CarExpenseImportMonth,
    CarExpenseImportYear
}

public static class ExcelImportExtensions
{
    #region Public Methods

    public static int GetStartRow(this ExcelImportType importType)
     => importType switch
     {
         ExcelImportType.CarExpenseImportMonth => 3,
         ExcelImportType.CarExpenseImportYear => 4,
         _ => throw new ArgumentException("Invalid Enum Value", nameof(importType))
     };

    #endregion
}