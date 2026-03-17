using CarCostCalculator.Domain.Model.Common;

namespace CarCostCalculator.Domain.Model.Kilometers;

public class KilometersChangeable : KilometersAddable, IAmIdentifiable
{
    #region Public Properties

    public long Id { get; set; }

    #endregion
}