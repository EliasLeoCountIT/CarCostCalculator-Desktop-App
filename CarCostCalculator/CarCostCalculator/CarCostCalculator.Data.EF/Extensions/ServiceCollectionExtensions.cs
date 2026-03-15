using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CarCostCalculator.Data.EF.Extensions;

public static class ServiceCollectionExtensions
{
    #region Public Methods

    public static IServiceCollection AddCarCostCalculatorContext(this IServiceCollection services, IConfiguration configuration)
        => services.AddDbContext<CarCostCalculatorContext>((_, opt)
            => opt.UseSqlServer(configuration.GetConnectionString(nameof(CarCostCalculatorContext))));

    #endregion
}