namespace CarCostCalculator.Domain.Model.CarExpense;

public class CarExpenseAddable
{
    #region Public Properties

    public decimal Amount { get; set; }

    public required DateOnly Date { get; set; }

    public required string ExpenseCategory { get; set; }

    public long ExpenseCategoryId { get; set; }

    public string? Notes { get; set; }

    #endregion
}