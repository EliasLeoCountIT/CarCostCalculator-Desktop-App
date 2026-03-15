using Microsoft.EntityFrameworkCore;

namespace CarCostCalculator.Data.EF.Entities;

public class CarExpense : IdentifiableBase
{
    #region Public Properties

    [Precision(18, 2)]
    public decimal CarInsurance { get; set; }

    public required DateOnly Date { get; set; }

    [Precision(18, 2)]
    public decimal Fuel { get; set; }

    [Precision(18, 2)]
    public decimal Inspection { get; set; }

    [Precision(18, 2)]
    public decimal KilometersDriven { get; set; }

    [Precision(18, 2)]
    public decimal OAMTC { get; set; }

    [Precision(18, 2)]
    public decimal Other { get; set; }

    [Precision(18, 2)]
    public decimal Registration { get; set; }

    [Precision(18, 2)]
    public decimal Service { get; set; }

    [Precision(18, 2)]
    public decimal Vignette { get; set; }

    #endregion
}