using Microsoft.EntityFrameworkCore;

namespace CarCostCalculator.Data.EF.Entities;

[PrimaryKey(nameof(Date))]
public class Kilometers : IdentifiableBase
{
    #region Public Properties

    public required DateOnly Date { get; set; }

    public bool IsDeleted { get; set; }

    [Precision(18, 2)]
    public decimal KilometersDriven { get; set; }

    #endregion
}