using AutoMapper;
using CarCostCalculator.Data.Contract;
using CarCostCalculator.Data.EF;
using CarCostCalculator.Data.EF.Entities;
using CarCostCalculator.Data.Repository.Base;
using CarCostCalculator.Domain.Model.Kilometers;
using Microsoft.EntityFrameworkCore;

namespace CarCostCalculator.Data.Repository;

public class KilometersRepository(CarCostCalculatorContext context, IMapper mapper) : BaseRepository<Kilometers, KilometersChangeable>(context, mapper), IKilometersRepository
{
    #region Private Members

    private readonly string _kilometerEntryNotFoundMessage = "No kilometer entry with Id: {0} found in DB";

    #endregion

    #region Public Methods

    public async Task<KilometersChangeable> Create(KilometersAddable kilometersAddable, CancellationToken cancellationToken)
    {
        var entity = Mapper.Map<Kilometers>(kilometersAddable);

        if (await Context.Kilometers.AnyAsync(e => e.Id == entity.Id, cancellationToken))
        {
            return await LoadByPrimaryKey(entity.Id, cancellationToken);
        }

        await Context.Kilometers.AddAsync(entity, cancellationToken);
        await Context.SaveChangesAsync(cancellationToken);

        return await LoadByPrimaryKey(entity.Id, cancellationToken)
            ?? throw new KeyNotFoundException(string.Format($"{_kilometerEntryNotFoundMessage} after adding", entity.Id));
    }

    public async Task<IEnumerable<KilometersChangeable>> CreateMany(IEnumerable<KilometersAddable> kilometers, CancellationToken cancellationToken)
    {
        var existingEntities = await QueryEntities()
            .ToListAsync(cancellationToken);

        var entitiesToCreate = Mapper.Map<IEnumerable<Kilometers>>(kilometers)
            .Where(e => !existingEntities.Any(existing => existing.Date == e.Date))
            .ToList();

        if (entitiesToCreate.Count > 0)
        {
            Context.Kilometers.AddRange(entitiesToCreate);
            await Context.SaveChangesAsync(cancellationToken);
        }

        return Mapper.Map<IEnumerable<KilometersChangeable>>(entitiesToCreate);
    }

    public async Task Delete(long id, CancellationToken cancellationToken)
    {
        var carExpenseToDelete = await Context.Kilometers.FirstOrDefaultAsync(a => a.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException(string.Format(_kilometerEntryNotFoundMessage, id));

        Context.Kilometers.Remove(carExpenseToDelete);
        await Context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<KilometersChangeable>> GetByMonth(int year, int month, CancellationToken cancellationToken)
    {
        var entities = await QueryEntities()
            .AsNoTracking()
            .Where(e => e.Date.Year == year && e.Date.Month == month)
            .OrderBy(e => e.Date)
            .ToListAsync(cancellationToken);

        return Mapper.Map<IEnumerable<KilometersChangeable>>(entities);
    }

    public async Task<IEnumerable<KilometersChangeable>> GetByTimeRange(DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken)
    {
        var entities = await QueryEntities()
            .AsNoTracking()
            .Where(e => e.Date >= startDate && e.Date <= endDate)
            .OrderBy(e => e.Date)
            .ToListAsync(cancellationToken);

        return Mapper.Map<IEnumerable<KilometersChangeable>>(entities);
    }

    public async Task<IEnumerable<KilometersChangeable>> GetByYear(int year, CancellationToken cancellationToken)
    {
        var entities = await QueryEntities()
            .AsNoTracking()
            .Where(e => e.Date.Year == year)
            .OrderBy(e => e.Date)
            .ToListAsync(cancellationToken);

        return Mapper.Map<IEnumerable<KilometersChangeable>>(entities);
    }

    public async Task<IEnumerable<KilometersChangeable>> LoadAll(CancellationToken cancellationToken)
    {
        var entities = await QueryEntities()
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return Mapper.Map<IEnumerable<KilometersChangeable>>(entities);
    }

    public async Task<KilometersChangeable> LoadByDate(DateOnly date, CancellationToken cancellationToken)
    {
        var entity = await QueryEntities()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Date == date, cancellationToken)
            ?? throw new KeyNotFoundException(string.Format(_kilometerEntryNotFoundMessage, date));

        return Mapper.Map<KilometersChangeable>(entity);
    }

    public async Task<KilometersChangeable> LoadByPrimaryKey(long id, CancellationToken cancellationToken)
    {
        var entity = await QueryEntities()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        return entity == null
            ? throw new KeyNotFoundException(string.Format(_kilometerEntryNotFoundMessage, id))
            : Mapper.Map<KilometersChangeable>(entity);
    }

    public async Task<KilometersChangeable> Update(KilometersChangeable KilometersChangeable, CancellationToken cancellationToken)
    {
        var existingEntity = await Context.Kilometers.FirstOrDefaultAsync(e => e.Id == KilometersChangeable.Id, cancellationToken)
            ?? throw new KeyNotFoundException(string.Format(_kilometerEntryNotFoundMessage, KilometersChangeable.Id));

        Mapper.Map(KilometersChangeable, existingEntity);
        await Context.SaveChangesAsync(cancellationToken);

        return await LoadByPrimaryKey(existingEntity.Id, cancellationToken);
    }

    #endregion
}