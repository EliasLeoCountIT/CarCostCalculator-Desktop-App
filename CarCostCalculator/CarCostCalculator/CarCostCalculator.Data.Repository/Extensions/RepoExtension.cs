using CarCostCalculator.Data.Contract;
using Microsoft.Extensions.DependencyInjection;

namespace CarCostCalculator.Data.Repository.Extensions;

public static class RepoExtension
{
    #region Public Methods

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICarExpenseRepository, CarExpenseRepository>();

        return services;
    }

    #endregion
}