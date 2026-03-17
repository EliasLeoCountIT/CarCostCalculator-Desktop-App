using CarCostCalculator.Domain.Model.Common;

namespace CarCostCalculator.Domain.Model.CarExpense;

public class CarExpenseChangeable : CarExpenseAddable, IAmIdentifiable
{
    #region Public Properties

    public long Id { get; set; }

    #endregion
}