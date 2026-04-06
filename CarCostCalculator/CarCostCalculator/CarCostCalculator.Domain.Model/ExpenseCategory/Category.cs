using System.Text.Json.Serialization;

namespace CarCostCalculator.Domain.Model.ExpenseCategory;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Category
{
    CarInsurance = 0,
    Registration = 1,
    Inspection = 2,
    Service = 3,
    OAMTC = 4,
    Vignette = 5,
    Fuel = 6,
    Other = 7,
}