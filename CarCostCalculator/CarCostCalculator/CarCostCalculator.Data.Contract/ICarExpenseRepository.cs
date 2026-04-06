using CarCostCalculator.Domain.Model.CarExpense;

namespace CarCostCalculator.Data.Contract;

public interface ICarExpenseRepository : IQueryableRepository<CarExpenseChangeable>
{
    #region Public Methods

    Task<CarExpenseChangeable> Create(CarExpenseAddable carExpenseAddable, CancellationToken cancellationToken);

    Task<IEnumerable<CarExpenseChangeable>> CreateMany(IEnumerable<CarExpenseAddable> carExpenses, CancellationToken cancellationToken);

    Task Delete(long id, CancellationToken cancellationToken);

    Task<IEnumerable<CarExpenseChangeable>> GetByMonth(int year, int month, CancellationToken cancellationToken);

    Task<IEnumerable<CarExpenseChangeable>> GetByTimeRange(DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken);

    Task<IEnumerable<CarExpenseChangeable>> GetByYear(int year, CancellationToken cancellationToken);

    Task<IEnumerable<CarExpenseChangeable>> LoadAll(CancellationToken cancellationToken);

    Task<CarExpenseChangeable> LoadByDate(DateOnly date, CancellationToken cancellationToken);

    Task<CarExpenseChangeable> LoadByPrimaryKey(long id, CancellationToken cancellationToken);

    Task<CarExpenseChangeable> Update(CarExpenseChangeable carExpenseChangeable, CancellationToken cancellationToken);

    #endregion
}