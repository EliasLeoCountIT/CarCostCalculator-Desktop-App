using AutoMapper;
using CarCostCalculator.Data.Contract;
using CarCostCalculator.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace CarCostCalculator.Data.Repository.Base;

public abstract class BaseRepository<TEntity, TContract>(CarCostCalculatorContext context, IMapper mapper)
    : IQueryableRepository<TContract>
    where TEntity : class
    where TContract : class
{
    #region Protected Properties

    protected CarCostCalculatorContext Context { get; } = context;

    protected IMapper Mapper { get; } = mapper;

    #endregion

    #region Public Methods

    public IQueryable<TContract> QueryProjection()
    {
        var query = Query();

        return Mapper.ProjectTo<TContract>(query);
    }

    #endregion

    #region Protected Methods

    protected virtual IQueryable<TEntity> ApplyDefaultIncludes(IQueryable<TEntity> query) => query;

    protected IQueryable<TEntity> QueryEntities()
    {
        var query = Query();

        return ApplyDefaultIncludes(query);
    }

    #endregion

    #region Private Methods

    private IQueryable<TEntity> Query() => Context.Set<TEntity>().AsNoTracking();

    #endregion
}