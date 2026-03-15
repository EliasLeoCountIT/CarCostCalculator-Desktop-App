using Microsoft.EntityFrameworkCore;

namespace CarCostCalculator.Data.EF.Entities;

[PrimaryKey(nameof(Id))]
public abstract class IdentifiableBase
{
    #region Public Properties

    public long Id { get; set; }

    #endregion
}