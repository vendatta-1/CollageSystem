using CollageSystem.Core.Validation;
using Microsoft.Extensions.Logging;

namespace CollageSystem.Core.Results;
/// <summary>
/// Represents the result of an operation, containing its status and any associated errors.
/// </summary>
public class OperationResult
{
    private readonly ILogger<OperationResult> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="OperationResult"/> class with the specified status.
    /// </summary>
    /// <param name="status">The initial status of the operation result.</param>
    /// <param name="logger">The logger instance used for logging error messages.</param>
    public OperationResult(OperationStatus status, ILogger<OperationResult> logger)
    {
        Status = status;
        _logger = logger;
    }

    /// <summary>
    /// Gets the status of the operation result.
    /// </summary>
    public OperationStatus Status { get; private set; }

    /// <summary>
    /// Gets the list of errors associated with the operation result.
    /// </summary>
    public List<ErrorDetail> Errors { get; private set; } = new();

    /// <summary>
    /// Gets a value indicating whether the operation was successful (i.e., no errors and status is Success).
    /// </summary>
    public bool IsSuccess => Status == OperationStatus.Success && !Errors.Any();

    /// <summary>
    /// Gets a value indicating whether the operation has any errors.
    /// </summary>
    public bool IsFailure => Status == OperationStatus.Failure || Errors.Count > 0;

    /// <summary>
    /// Sets the status of the operation result to the specified value.
    /// </summary>
    /// <param name="status">The new status of the operation result.</param>
    /// <returns>The current <see cref="OperationResult"/> instance.</returns>
    public OperationResult WithStatus(OperationStatus status)
    {
        Status = status;
        _logger.LogInformation($"Operation status set to: {status}");
        return this;
    }

    /// <summary>
    /// Adds an error to the operation result using the specified error code and failure level.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="level">The failure level of the error.</param>
    /// <returns>The current <see cref="OperationResult"/> instance.</returns>
    public OperationResult WithErrorCode(ErrorCode code, FailureLevel level = FailureLevel.Important)
    {
        var message = ErrorMessages.GetMessage(code);
        Errors.Add(new ErrorDetail(code, message, level));

        if (level == FailureLevel.Critical)
        {
            Status = OperationStatus.Failure;
            _logger.LogError($"Critical error occurred: Code = {code}, Message = {message}");
        }
        else
        {
            _logger.LogWarning($"Error occurred: Code = {code}, Message = {message}");
        }

        return this;
    }

    /// <summary>
    /// Adds an error to the operation result using the specified error code, custom message, and failure level.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="customMessage">The custom error message.</param>
    /// <param name="level">The failure level of the error.</param>
    /// <returns>The current <see cref="OperationResult"/> instance.</returns>
    public OperationResult WithErrorCode(ErrorCode code, string customMessage, FailureLevel level)
    {
        Errors.Add(new ErrorDetail(code, customMessage, level));

        if (level == FailureLevel.Critical)
        {
            Status = OperationStatus.Failure;
            _logger.LogError($"Critical error occurred: Code = {code}, Message = {customMessage}");
        }
        else
        {
            _logger.LogWarning($"Error occurred: Code = {code}, Message = {customMessage}");
        }

        return this;
    }

    /// <summary>
    /// Adds an error to the operation result using the specified exception.
    /// </summary>
    /// <param name="ex">The exception to include in the error details.</param>
    /// <param name="level">The failure level of the error.</param>
    /// <returns>The current <see cref="OperationResult"/> instance.</returns>
    public OperationResult WithException(Exception ex, FailureLevel level = FailureLevel.Critical)
    {
        var code = ErrorCode.GeneralError; // Use a default or generic error code
        var message = ex.Message;
        Errors.Add(new ErrorDetail(code, message, level));

        if (level == FailureLevel.Critical)
        {
            Status = OperationStatus.Failure;
            _logger.LogError($"Critical error occurred: Exception = {ex}");
        }
        else
        {
            _logger.LogWarning($"Error occurred: Exception = {ex}");
        }

        return this;
    }

    /// <summary>
    /// Clears all errors from the operation result.
    /// </summary>
    /// <returns>The current <see cref="OperationResult"/> instance.</returns>
    public OperationResult ClearErrors()
    {
        Errors.Clear();
        _logger.LogInformation("All errors have been cleared.");
        return this;
    }
}
