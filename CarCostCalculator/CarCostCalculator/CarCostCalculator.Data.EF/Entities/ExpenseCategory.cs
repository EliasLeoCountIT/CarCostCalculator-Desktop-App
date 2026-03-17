using System.ComponentModel.DataAnnotations;

namespace CarCostCalculator.Data.EF.Entities;

public class ExpenseCategory : IdentifiableBase
{
    #region Public Properties

    public virtual ICollection<CarExpense>? CarExpenses { get; set; }

    [MaxLength(50)]
    public required string Name { get; set; }

    #endregion
}