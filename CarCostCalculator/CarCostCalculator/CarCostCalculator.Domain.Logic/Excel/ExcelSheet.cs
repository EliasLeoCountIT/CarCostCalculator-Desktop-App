using CarCostCalculator.Domain.Logic.Excel.Models;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;

namespace CarCostCalculator.Domain.Logic.Excel;

public partial class ExcelSheet(ExcelImportType importType, IDictionary<string, string?> data, string name)
{
    #region Private Members

    private readonly IReadOnlyDictionary<string, string?> _data = new ReadOnlyDictionary<string, string?>(data);

    private int? _maxRow;

    #endregion

    #region Public Properties

    public ExcelImportType ImportType { get; set; } = importType;

    public string Name { get; set; } = name;

    #endregion

    #region Public Indexers

    public string? this[string id] => _data.GetValueOrDefault(id);

    public string? this[string letter, int number] => _data.GetValueOrDefault(letter + number);

    public string? this[int letterAsNumber, int number] => _data.GetValueOrDefault(ConvertToCoordinateLetters(letterAsNumber) + number);

    #endregion

    #region Public Methods

    public static string ConvertToCoordinateLetters(int number)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(number, 0);

        number--;
        var result = new StringBuilder();

        while (number >= 0)
        {
            var remainder = number % 26;
            result.Insert(0, (char)('A' + remainder));
            number = ((number - remainder) / 26) - 1;
        }

        return result.ToString();
    }

    public int GetMaxRow()
    {
        if (_maxRow.HasValue)
        {
            return _maxRow.Value;
        }

        // Get the start row based on import type
        var startRow = ImportType.GetStartRow();

        // Read the first date cell to determine the month
        var firstDateCell = this["B", startRow];

        if (string.IsNullOrWhiteSpace(firstDateCell) || !DateOnly.TryParse(firstDateCell, out var firstDate))
        {
            // Fallback to scanning all cells if date parsing fails
            _maxRow = _data.Max(c => GetRowNumber(c.Key));
            return _maxRow.Value;
        }

        // Calculate the number of days in the month
        var daysInMonth = DateTime.DaysInMonth(firstDate.Year, firstDate.Month);

        // Max row = start row + days in month - 1 (since start row is the first day)
        _maxRow = startRow + daysInMonth - 1;

        return _maxRow.Value;
    }

    #endregion

    #region Private Methods

    private static int GetRowNumber(string id)
    {
        var regex = NumberPattern();
        var match = regex.Match(id);

        if (!match.Success)
        {
            throw new InvalidOperationException($"No number found in the cell id: '{id}'.");
        }

        return int.Parse(match.Value);
    }

    [GeneratedRegex("[0-9]+")]
    private static partial Regex NumberPattern();

    #endregion
}