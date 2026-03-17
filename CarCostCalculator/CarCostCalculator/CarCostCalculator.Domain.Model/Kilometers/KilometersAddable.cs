namespace CarCostCalculator.Domain.Model.Kilometers;

public class KilometersAddable
{
    #region Public Properties

    public required DateOnly Date { get; set; }

    public decimal KilometersDriven { get; set; }

    #endregion
}