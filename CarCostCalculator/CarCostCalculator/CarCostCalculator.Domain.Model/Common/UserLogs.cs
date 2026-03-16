using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace CarCostCalculator.Domain.Model.Common;

/// <summary>
/// Logs which are returned to the user
/// </summary>
public class UserLogs
{
    #region Public Properties

    public Collection<string> Errors { get; set; } = [];

    public Collection<string> Information { get; set; } = [];

    public Collection<string> Warnings { get; set; } = [];

    #endregion

    #region Public Methods

    /// <summary>
    /// Logs and stores an error. The logger is taken from the caller to keep the scope.
    /// </summary>
    /// <param name="logger">The logger instance from the caller.</param>
    /// <param name="message">The message to be added.</param>
    public void LogError(ILogger logger, string message, Exception? ex = null)
    {
        Errors.Add(message);

        if (ex is null)
        {
            logger.LogError("{UserLogMessage}", message);
        }
        else
        {
            logger.LogError(ex, "{UserLogMessage}", message);
        }
    }

    /// <summary>
    /// Logs and stores an information. The logger is taken from the caller to keep the scope.
    /// </summary>
    /// <param name="logger">The logger instance from the caller.</param>
    /// <param name="message">The message to be added.</param>
    [SuppressMessage("Performance", "CA1873:Avoid potentially expensive logging", Justification = "Needed for this logging scenario")]
    public void LogInformation(ILogger logger, string message)
    {
        Information.Add(message);
        logger.LogInformation("{UserLogMessage}", message);
    }

    /// <summary>
    /// Logs and stores a warning. The logger is taken from the caller to keep the scope.
    /// </summary>
    /// <param name="logger">The logger instance from the caller.</param>
    /// <param name="message">The message to be added.</param>
    public void LogWarning(ILogger logger, string message)
    {
        Warnings.Add(message);
        logger.LogWarning("{UserLogMessage}", message);
    }

    #endregion
}