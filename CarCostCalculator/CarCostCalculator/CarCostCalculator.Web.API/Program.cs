using CarCostCalculator.Data.EF.Extensions;
using CarCostCalculator.Data.Repository.Extensions;
using CarCostCalculator.Data.Repository.Profiles;

namespace CarCostCalculator.Web.API;

public static class Program
{
    #region Public Methods

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Add services to the container.
        builder.Services.AddAuthorization();

        // add data access layer
        builder.Services
            .AddCarCostCalculatorContext(builder.Configuration)
            .AddAutoMapper(_ => { }, typeof(CarExpenseProfile).Assembly)
            .AddRepositories();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Services.AddControllers();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }

    #endregion
}