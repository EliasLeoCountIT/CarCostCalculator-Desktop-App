namespace CarCostCalculator.Common;

/// <summary>
/// Represents a file that is sent in a command.
/// </summary>
/// <param name="openReadStream">Retrieve the stream for reading the file.</param>
/// <param name="fileName">File name of the corresponding file.</param>
/// <param name="contentType">Content type of the corresponding file.</param>
/// <param name="length">Length of the corresponding file.</param>
/// <param name="contentDisposition">Additional info of the corresponding file.</param>
public class FileCommandItem(Func<Stream> openReadStream, string fileName, string contentType, long? length = null, string contentDisposition = "form-data")
{
    #region Private Members

    private readonly Func<Stream> _openReadStream = openReadStream;

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets additional info of the corresponding file.
    /// </summary>
    public string ContentDisposition { get; } = contentDisposition;

    /// <summary>
    /// Gets the content type of the corresponding file.
    /// </summary>
    public string ContentType { get; } = contentType;

    /// <summary>
    /// Gets the file name of the corresponding file.
    /// </summary>
    public string FileName { get; } = fileName;

    /// <summary>
    /// Gets the length of the corresponding file.
    /// </summary>
    public long? Length { get; } = length;

    #endregion

    #region Public Methods

    /// <summary>
    /// Opens a stream for the corresponding file.
    /// </summary>
    /// <remarks>
    /// This stream needs to be disposed!
    /// </remarks>
    public Stream OpenReadStream() => _openReadStream();

    #endregion
}