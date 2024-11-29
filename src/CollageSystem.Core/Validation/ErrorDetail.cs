

namespace CollageSystem.Core.Validation;

public record ErrorDetail(ErrorCode Code, string Message, FailureLevel Level);