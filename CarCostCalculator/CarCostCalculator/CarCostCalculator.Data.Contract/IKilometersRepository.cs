using CarCostCalculator.Domain.Model.Kilometers;

namespace CarCostCalculator.Data.Contract;

public interface IKilometersRepository : IQueryableRepository<KilometersChangeable>
{
    #region Public Methods

    Task<KilometersChangeable> Add(KilometersAddable kilometersAddable, CancellationToken cancellationToken);

    Task Delete(long id, CancellationToken cancellationToken);

    Task<IEnumerable<KilometersChangeable>> GetByMonth(int year, int month, CancellationToken cancellationToken);

    Task<IEnumerable<KilometersChangeable>> GetByTimeRange(DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken);

    Task<IEnumerable<KilometersChangeable>> GetByYear(int year, CancellationToken cancellationToken);

    Task<IEnumerable<KilometersChangeable>> LoadAll(CancellationToken cancellationToken);

    Task<KilometersChangeable> LoadByDate(DateOnly date, CancellationToken cancellationToken);

    Task<KilometersChangeable> LoadByPrimaryKey(long id, CancellationToken cancellationToken);

    Task<KilometersChangeable> Update(KilometersChangeable KilometersChangeable, CancellationToken cancellationToken);

    #endregion
}