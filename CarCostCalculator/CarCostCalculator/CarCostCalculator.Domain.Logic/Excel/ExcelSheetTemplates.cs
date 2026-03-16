using CarCostCalculator.Domain.Logic.Excel.Models;
using System.Collections.ObjectModel;

namespace CarCostCalculator.Domain.Logic.Excel;

public static class ExcelSheetTemplates
{
    #region Public Properties

    public static ExcelSheetTemplate AnnualCarExpenseTemplate => new(ExcelImportType.CarExpenseImportYear, new ReadOnlyDictionary<string, string>(new Dictionary<string, string>
    {
        { "B4", "Monat" },
        { "C4", "Datum" },
        { "D4", "Kfz-Versicherung" },
        { "E4", "Zulassung" },
        { "F4", "Pickerl" },
        { "G4", "Service" },
        { "H4", "ÖAMTC" },
        { "I4", "Vignette" },
        { "J4", "Tanken" },
        { "K4", "Sonstiges" },
        { "L4", "Gefahrene Kilometer" }
    }));

    public static ExcelSheetTemplate MonthlyCarExpenseTemplate => new(ExcelImportType.CarExpenseImportMonth, new ReadOnlyDictionary<string, string>(new Dictionary<string, string>
    {
        { "B2", "Datum" },
        { "C2", "Kfz-Versicherung" },
        { "D2", "Zulassung" },
        { "E2", "Pickerl" },
        { "F2", "Service" },
        { "G2", "ÖAMTC" },
        { "H2", "Vignette" },
        { "I2", "Tanken" },
        { "J2", "Sonstiges" },
        { "K2", "Gefahrene Kilometer" }
    }));

    #endregion
}