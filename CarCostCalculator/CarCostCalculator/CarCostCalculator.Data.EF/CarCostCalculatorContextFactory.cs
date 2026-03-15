using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CarCostCalculator.Data.EF;

public class CarCostCalculatorContextFactory : IDesignTimeDbContextFactory<CarCostCalculatorContext>
{
    #region Public Methods

    public CarCostCalculatorContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CarCostCalculatorContext>();

        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=CarCostCalculatorApp;Trusted_Connection=True;");

        return new CarCostCalculatorContext(optionsBuilder.Options);
    }

    #endregion
}