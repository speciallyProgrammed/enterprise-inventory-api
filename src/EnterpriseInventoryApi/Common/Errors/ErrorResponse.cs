namespace EnterpriseInventoryApi.Common.Errors;

public class ErrorResponse
{
    public ErrorBody Error { get; set; } = new ErrorBody();

    public static ErrorResponse FromException(string code, string message, string traceId)
    {
        return new ErrorResponse
        {
            Error = new ErrorBody
            {
                Code = code,
                Message = message,
                TraceId = traceId
            }
        };
    }

    public static ErrorResponse FromValidation(IDictionary<string, string[]> errors, string traceId)
    {
        return new ErrorResponse
        {
            Error = new ErrorBody
            {
                Code = ErrorCodes.ValidationError,
                Message = "Validation failed",
                TraceId = traceId,
                Details = errors
            }
        };
    }
}

public class ErrorBody
{
    public string Code { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string TraceId { get; set; } = string.Empty;
    public object? Details { get; set; }
}
