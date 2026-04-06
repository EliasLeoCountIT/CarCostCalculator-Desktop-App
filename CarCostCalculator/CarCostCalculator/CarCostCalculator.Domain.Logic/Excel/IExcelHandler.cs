using CarCostCalculator.Common;
using CarCostCalculator.Domain.Logic.Excel.Models;
using CarCostCalculator.Domain.Model.Common;

namespace CarCostCalculator.Domain.Logic.Excel;

public interface IExcelHandler
{
    #region Public Methods

    List<DailyDataFromExcel> GetCarExpenseData(ExcelSheet sheet, UserLogs userLogs);

    ICollection<ExcelSheet> LoadData(FileCommandItem file, UserLogs userLogs);

    #endregion
}