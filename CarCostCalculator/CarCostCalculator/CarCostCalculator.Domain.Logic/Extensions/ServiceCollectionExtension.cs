using CarCostCalculator.Domain.Logic.Excel;
using Microsoft.Extensions.DependencyInjection;

namespace CarCostCalculator.Domain.Logic.Extensions;

public static class ServiceCollectionExtension
{
    #region Public Methods

    public static IServiceCollection AddHandlerHelper(this IServiceCollection services)
    {
        services.AddScoped<IExcelHandler, ExcelHandler>();
        return services;
    }

    #endregion
}