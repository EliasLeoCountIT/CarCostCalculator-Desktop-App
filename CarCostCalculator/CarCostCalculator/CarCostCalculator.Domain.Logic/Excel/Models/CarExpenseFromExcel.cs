using CarCostCalculator.Domain.Logic.Excel.Models.Base;

namespace CarCostCalculator.Domain.Logic.Excel.Models;

public class CarExpenseFromExcel : BaseFromExcel
{
    #region Public Properties

    public decimal CarInsurance { get; set; }

    public required DateOnly Date { get; set; }

    public decimal Fuel { get; set; }

    public decimal Inspection { get; set; }

    public decimal KilometersDriven { get; set; }

    public decimal OAMTC { get; set; }

    public decimal Other { get; set; }

    public decimal Registration { get; set; }

    public decimal Service { get; set; }

    public decimal Vignette { get; set; }

    #endregion
}