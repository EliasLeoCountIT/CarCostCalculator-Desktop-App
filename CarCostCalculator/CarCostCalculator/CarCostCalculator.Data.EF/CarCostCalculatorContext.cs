using CarCostCalculator.Data.EF.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Reflection;

namespace CarCostCalculator.Data.EF;

public class CarCostCalculatorContext(DbContextOptions<CarCostCalculatorContext> options) : DbContext(options)
{
    #region Public Properties

    public DbSet<CarExpense> CarExpenses => Set<CarExpense>();

    public DbSet<ExpenseCategory> ExpenseCategories => Set<ExpenseCategory>();

    public DbSet<Kilometers> Kilometers => Set<Kilometers>();

    #endregion

    #region Protected Methods

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Conventions.Remove<TableNameFromDbSetConvention>();
        configurationBuilder.Properties<Enum>().HaveConversion<string>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

    #endregion
}