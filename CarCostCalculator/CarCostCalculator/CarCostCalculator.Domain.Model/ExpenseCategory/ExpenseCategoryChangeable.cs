using CarCostCalculator.Domain.Model.Common;

namespace CarCostCalculator.Domain.Model.ExpenseCategory;

public class ExpenseCategoryChangeable : ExpenseCategoryAddable, IAmIdentifiable
{
    #region Public Properties

    public long Id { get; set; }

    #endregion
}