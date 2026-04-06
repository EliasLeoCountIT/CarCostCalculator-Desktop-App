using CarCostCalculator.Domain.Model.ExpenseCategory;

namespace CarCostCalculator.Data.Contract;

public interface IExpenseCategoryRepository : IQueryableRepository<ExpenseCategoryChangeable>
{
    #region Public Methods

    Task<ExpenseCategoryChangeable> Create(ExpenseCategoryAddable expenseCategoryAddable, CancellationToken cancellationToken);

    Task<IEnumerable<ExpenseCategoryChangeable>> CreateMany(IEnumerable<ExpenseCategoryAddable> expenseCategories, CancellationToken cancellationToken);

    Task Delete(long id, CancellationToken cancellationToken);

    Task<IEnumerable<ExpenseCategoryChangeable>> LoadAll(CancellationToken cancellationToken);

    Task<ExpenseCategoryChangeable> LoadByPrimaryKey(long id, CancellationToken cancellationToken);

    Task<ExpenseCategoryChangeable> Update(ExpenseCategoryChangeable expenseCategoryChangeable, CancellationToken cancellationToken);

    #endregion
}