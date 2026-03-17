using AutoMapper;
using CarCostCalculator.Data.Contract;
using CarCostCalculator.Data.EF;
using CarCostCalculator.Data.EF.Entities;
using CarCostCalculator.Data.Repository.Base;
using CarCostCalculator.Domain.Model.ExpenseCategory;
using Microsoft.EntityFrameworkCore;

namespace CarCostCalculator.Data.Repository;

public class ExpenseCategoryRepository(CarCostCalculatorContext context, IMapper mapper) : BaseRepository<ExpenseCategory, ExpenseCategoryChangeable>(context, mapper), IExpenseCategoryRepository
{
    #region Private Members

    private readonly string _expenseCategoryNotFoundMessage = "No expense category with Id: {0} found in DB";

    #endregion

    #region Public Methods

    public async Task<ExpenseCategoryChangeable> Add(ExpenseCategoryAddable expenseCategoryAddable, CancellationToken cancellationToken)
    {
        var entity = Mapper.Map<ExpenseCategory>(expenseCategoryAddable);

        if (await Context.ExpenseCategories.AnyAsync(e => e.Id == entity.Id, cancellationToken))
        {
            return await LoadByPrimaryKey(entity.Id, cancellationToken);
        }

        await Context.ExpenseCategories.AddAsync(entity, cancellationToken);
        await Context.SaveChangesAsync(cancellationToken);

        return await LoadByPrimaryKey(entity.Id, cancellationToken)
            ?? throw new KeyNotFoundException(string.Format($"{_expenseCategoryNotFoundMessage} after adding", entity.Id));
    }

    public async Task Delete(long id, CancellationToken cancellationToken)
    {
        var carExpenseCategoryToDelete = await Context.ExpenseCategories.FirstOrDefaultAsync(a => a.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException(string.Format(_expenseCategoryNotFoundMessage, id));

        Context.ExpenseCategories.Remove(carExpenseCategoryToDelete);
        await Context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<ExpenseCategoryChangeable>> LoadAll(CancellationToken cancellationToken)
    {
        var entities = await QueryEntities()
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return Mapper.Map<IEnumerable<ExpenseCategoryChangeable>>(entities);
    }

    public async Task<ExpenseCategoryChangeable> LoadByPrimaryKey(long id, CancellationToken cancellationToken)
    {
        var entity = await QueryEntities()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        return entity == null
            ? throw new KeyNotFoundException(string.Format(_expenseCategoryNotFoundMessage, id))
            : Mapper.Map<ExpenseCategoryChangeable>(entity);
    }

    public async Task<ExpenseCategoryChangeable> Update(ExpenseCategoryChangeable expenseCategoryChangeable, CancellationToken cancellationToken)
    {
        var existingEntity = await Context.ExpenseCategories.FirstOrDefaultAsync(e => e.Id == expenseCategoryChangeable.Id, cancellationToken)
           ?? throw new KeyNotFoundException(string.Format(_expenseCategoryNotFoundMessage, expenseCategoryChangeable.Id));

        Mapper.Map(expenseCategoryChangeable, existingEntity);
        await Context.SaveChangesAsync(cancellationToken);

        return await LoadByPrimaryKey(existingEntity.Id, cancellationToken);
    }

    #endregion
}