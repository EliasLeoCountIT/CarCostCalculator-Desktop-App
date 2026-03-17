using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarCostCalculator.Data.EF.Entities;

public class CarExpense : IdentifiableBase
{
    #region Public Properties

    [Precision(18, 2)]
    public decimal Amount { get; set; }

    public required DateOnly Date { get; set; }

    public virtual ExpenseCategory? ExpenseCategory { get; set; }

    [ForeignKey(nameof(ExpenseCategory))]
    public long ExpenseCategoryId { get; set; }

    public bool IsDeleted { get; set; }

    [MaxLength(100)]
    public string? Notes { get; set; }

    #endregion
}