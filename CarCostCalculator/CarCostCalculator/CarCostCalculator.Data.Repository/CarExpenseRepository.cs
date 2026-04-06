using AutoMapper;
using CarCostCalculator.Data.Contract;
using CarCostCalculator.Data.EF;
using CarCostCalculator.Data.EF.Entities;
using CarCostCalculator.Data.Repository.Base;
using CarCostCalculator.Domain.Model.CarExpense;
using Microsoft.EntityFrameworkCore;

namespace CarCostCalculator.Data.Repository;

public class CarExpenseRepository(CarCostCalculatorContext context, IMapper mapper) : BaseRepository<CarExpense, CarExpenseChangeable>(context, mapper), ICarExpenseRepository
{
    #region Private Members

    private readonly string _carExpenseNotFoundMessage = "No car expense with Id: {0} found in DB";

    #endregion

    #region Public Methods

    public async Task<CarExpenseChangeable> Create(CarExpenseAddable carExpenseAddable, CancellationToken cancellationToken)
    {
        var entity = Mapper.Map<CarExpense>(carExpenseAddable);

        if (await Context.CarExpenses.AnyAsync(e => e.Id == entity.Id, cancellationToken))
        {
            return await LoadByPrimaryKey(entity.Id, cancellationToken);
        }

        await Context.CarExpenses.AddAsync(entity, cancellationToken);
        await Context.SaveChangesAsync(cancellationToken);

        return await LoadByPrimaryKey(entity.Id, cancellationToken)
            ?? throw new KeyNotFoundException(string.Format($"{_carExpenseNotFoundMessage} after adding", entity.Id));
    }

    public async Task<IEnumerable<CarExpenseChangeable>> CreateMany(IEnumerable<CarExpenseAddable> carExpenses, CancellationToken cancellationToken)
    {
        var entities = Mapper.Map<IEnumerable<CarExpense>>(carExpenses);

        if (entities.Any())
        {
            Context.CarExpenses.AddRange(entities);
            await Context.SaveChangesAsync(cancellationToken);
        }

        return Mapper.Map<IEnumerable<CarExpenseChangeable>>(entities);
    }

    public async Task Delete(long id, CancellationToken cancellationToken)
    {
        var carExpenseToDelete = await Context.CarExpenses.FirstOrDefaultAsync(a => a.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException(string.Format(_carExpenseNotFoundMessage, id));

        Context.CarExpenses.Remove(carExpenseToDelete);
        await Context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<CarExpenseChangeable>> GetByMonth(int year, int month, CancellationToken cancellationToken)
    {
        var entities = await QueryEntities()
            .AsNoTracking()
            .Where(e => e.Date.Year == year && e.Date.Month == month)
            .OrderBy(e => e.Date)
            .ToListAsync(cancellationToken);

        return Mapper.Map<IEnumerable<CarExpenseChangeable>>(entities);
    }

    public async Task<IEnumerable<CarExpenseChangeable>> GetByTimeRange(DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken)
    {
        var entities = await QueryEntities()
            .AsNoTracking()
            .Where(e => e.Date >= startDate && e.Date <= endDate)
            .OrderBy(e => e.Date)
            .ToListAsync(cancellationToken);

        return Mapper.Map<IEnumerable<CarExpenseChangeable>>(entities);
    }

    public async Task<IEnumerable<CarExpenseChangeable>> GetByYear(int year, CancellationToken cancellationToken)
    {
        var entities = await QueryEntities()
            .AsNoTracking()
            .Where(e => e.Date.Year == year)
            .OrderBy(e => e.Date)
            .ToListAsync(cancellationToken);

        return Mapper.Map<IEnumerable<CarExpenseChangeable>>(entities);
    }

    public async Task<IEnumerable<CarExpenseChangeable>> LoadAll(CancellationToken cancellationToken)
    {
        var entities = await QueryEntities()
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return Mapper.Map<IEnumerable<CarExpenseChangeable>>(entities);
    }

    public async Task<CarExpenseChangeable> LoadByDate(DateOnly date, CancellationToken cancellationToken)
    {
        var entity = await QueryEntities()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Date == date, cancellationToken)
            ?? throw new KeyNotFoundException(string.Format(_carExpenseNotFoundMessage, date));

        return Mapper.Map<CarExpenseChangeable>(entity);
    }

    public async Task<CarExpenseChangeable> LoadByPrimaryKey(long id, CancellationToken cancellationToken)
    {
        var entity = await QueryEntities()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        return entity == null
            ? throw new KeyNotFoundException(string.Format(_carExpenseNotFoundMessage, id))
            : Mapper.Map<CarExpenseChangeable>(entity);
    }

    public async Task<CarExpenseChangeable> Update(CarExpenseChangeable carExpenseChangeable, CancellationToken cancellationToken)
    {
        var existingEntity = await Context.CarExpenses.FirstOrDefaultAsync(e => e.Id == carExpenseChangeable.Id, cancellationToken)
            ?? throw new KeyNotFoundException(string.Format(_carExpenseNotFoundMessage, carExpenseChangeable.Id));

        Mapper.Map(carExpenseChangeable, existingEntity);
        await Context.SaveChangesAsync(cancellationToken);

        return await LoadByPrimaryKey(existingEntity.Id, cancellationToken);
    }

    #endregion
}