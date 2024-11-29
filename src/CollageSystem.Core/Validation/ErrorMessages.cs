namespace CollageSystem.Core.Validation
{
    /// <summary>
    /// Provides error messages for the given error codes.
    /// </summary>
    public static class ErrorMessages
    {
        #region Authorization & Authentication
        /// <summary>
        /// Gets the error message associated with an error code.
        /// </summary>
        /// <param name="code">The error code.</param>
        /// <returns>The error message.</returns>
        public static string GetMessage(ErrorCode code) => code switch
        {
            ErrorCode.UnauthorizedAccess => "Access is denied due to unauthorized access.",
            ErrorCode.InvalidCredentials => "The provided credentials are invalid.",
            ErrorCode.AccountLocked => "The account is locked.",
            ErrorCode.SessionExpired => "The session has expired.",
            ErrorCode.TokenInvalid => "The token is invalid.",
            ErrorCode.TokenExpired => "The token has expired.",
            ErrorCode.InvalidTokenFormat => "The token format is invalid.",
            ErrorCode.InsufficientPermissions => "Insufficient permissions to perform this operation.",
            ErrorCode.PasswordTooWeak => "The provided password is too weak.",
            ErrorCode.AccountNotActivated => "The account is not activated.",
            ErrorCode.AccountSuspended => "The account is suspended.",
            ErrorCode.EmailVerificationFailed => "Email verification failed.",
            ErrorCode.MultiFactorAuthenticationRequired => "Multi-factor authentication is required.",
            ErrorCode.AccountAlreadyExists => "An account with this information already exists.",
            ErrorCode.PasswordResetFailed => "Password reset failed.",
            ErrorCode.AuthServiceUnavailable => "Authentication service is currently unavailable.",
            ErrorCode.InvalidRefreshToken => "The refresh token is invalid.",
            ErrorCode.AccountDeactivated => "The account has been deactivated.",
            ErrorCode.AccountVerificationPending => "The account verification is pending.",
            ErrorCode.InvalidTwoFactorCode => "The provided two-factor code is invalid.",
            ErrorCode.UntrustedDevice => "Access attempt from an untrusted device.",
            
        

        #endregion

        #region CRUD Operations
        
            ErrorCode.CreateFailed => "Failed to create the record.",
            ErrorCode.ReadFailed => "Failed to read the record.",
            ErrorCode.UpdateFailed => "Failed to update the record.",
            ErrorCode.DeleteFailed => "Failed to delete the record.",
            ErrorCode.NotFound => "The record was not found.",
            ErrorCode.DuplicateRecord => "A record with the same identifier already exists.",
            ErrorCode.RecordLocked => "The record is currently locked and cannot be modified.",
            ErrorCode.OperationNotSupported => "The requested operation is not supported.",
            ErrorCode.DataConflict => "Data conflict occurred during the operation.",
            ErrorCode.DataIntegrityViolation => "Data integrity violation detected.",
            ErrorCode.RecordAlreadyExists => "The record already exists.",
            ErrorCode.CreateNoChanges => "No changes were made during the create operation.",
            ErrorCode.UpdateNoChanges => "No changes were made during the update operation.",
            ErrorCode.DeleteNoChanges => "No changes were made during the delete operation.",
            ErrorCode.ReadAccessDenied => "Access to read the record is denied.",
            ErrorCode.CreateAccessDenied => "Access to create the record is denied.",
            ErrorCode.UpdateAccessDenied => "Access to update the record is denied.",
            ErrorCode.DeleteAccessDenied => "Access to delete the record is denied.",
            ErrorCode.RecordAlreadyDeleted => "The record has already been deleted.",
            ErrorCode.ConcurrentModification => "The record was modified by another process.",
            
        
        #endregion

        #region Data Validation
        
            ErrorCode.ValidationFailed => "Data validation failed.",
            ErrorCode.DataFormatInvalid => "The data format is invalid.",
            ErrorCode.RequiredFieldMissing => "A required field is missing.",
            ErrorCode.MaxLengthExceeded => "The field exceeds the maximum length allowed.",
            ErrorCode.MinLengthRequired => "The field does not meet the minimum length requirement.",
            ErrorCode.InvalidEmailFormat => "The email address format is invalid.",
            ErrorCode.InvalidPhoneNumber => "The phone number format is invalid.",
            ErrorCode.ValueOutOfRange => "The value is out of the allowable range.",
            ErrorCode.UnsupportedDataType => "The data type is not supported.",
            ErrorCode.InvalidDateFormat => "The date format is invalid.",
            ErrorCode.InvalidCurrencyFormat => "The currency format is invalid.",
            ErrorCode.InvalidPostalCode => "The postal code format is invalid.",
            ErrorCode.InvalidGender => "The specified gender is invalid.",
            ErrorCode.UnsupportedFileType => "The file type is not supported.",
            ErrorCode.FieldTooShort => "The field value is too short.",
            ErrorCode.FieldTooLong => "The field value is too long.",
            ErrorCode.InvalidAge => "The age provided is invalid.",
            ErrorCode.DateOutOfRange => "The date is out of the allowable range.",
            ErrorCode.InvalidIPAddress => "The IP address format is invalid.",
            
        #endregion

        #region Business Logic
       
            ErrorCode.OperationNotAllowed => "The operation is not allowed.",
            ErrorCode.InsufficientFunds => "Insufficient funds for the operation.",
            ErrorCode.ResourceLimitExceeded => "The resource limit has been exceeded.",
            ErrorCode.BusinessRuleViolation => "A business rule has been violated.",
            ErrorCode.ResourceNotAvailable => "The requested resource is not available.",
            ErrorCode.ExceedsQuota => "The operation exceeds the allowed quota.",
            ErrorCode.RateLimitExceeded => "The rate limit for the operation has been exceeded.",
            ErrorCode.InvalidDiscountCode => "The discount code is invalid.",
            ErrorCode.SubscriptionExpired => "The subscription has expired.",
            ErrorCode.FeatureNotEnabled => "The requested feature is not enabled.",
            ErrorCode.PremiumFeatureRequired => "A premium feature is required for this operation.",
            ErrorCode.InvalidOrderState => "The order is in an invalid state.",
            ErrorCode.ProductOutOfStock => "The product is out of stock.",
            ErrorCode.ServiceUnavailable => "The service is currently unavailable.",
            ErrorCode.PaymentFailed => "Payment processing failed.",
            ErrorCode.PriceMismatch => "The price does not match the expected value.",
            ErrorCode.ContractViolation => "A contract violation has occurred.",
            ErrorCode.CustomerNotFound => "The customer record was not found.",
            
        
        #endregion

        #region Math Operations
        
            ErrorCode.DivisionByZero => "Attempted division by zero.",
            ErrorCode.Overflow => "A mathematical overflow occurred.",
            ErrorCode.Underflow => "A mathematical underflow occurred.",
            ErrorCode.CalculationError => "An error occurred during calculation.",
            ErrorCode.InvalidOperation => "The operation is not valid.",
            ErrorCode.PrecisionLoss => "Loss of precision during calculation.",
            ErrorCode.UnsupportedMathOperation => "The mathematical operation is not supported.",
            ErrorCode.InvalidRounding => "The rounding operation is invalid.",
            ErrorCode.ComplexNumberNotSupported => "Complex numbers are not supported.",
            ErrorCode.DecimalConversionError => "Error occurred during decimal conversion.",
            ErrorCode.FloatingPointError => "An error occurred with floating point operations.",
            ErrorCode.SquareRootNegative => "Attempted to take the square root of a negative number.",
            ErrorCode.LogarithmNegative => "Attempted to compute the logarithm of a negative number.",
            ErrorCode.ExponentiationError => "An error occurred during exponentiation.",
            ErrorCode.InvalidMatrixOperation => "The matrix operation is invalid.",
            ErrorCode.StatisticalCalculationError => "An error occurred during statistical calculations.",
          
        #endregion

        #region File Operations
        
            ErrorCode.FileNotFound => "The file was not found.",
            ErrorCode.FileAccessDenied => "Access to the file is denied.",
            ErrorCode.FileFormatUnsupported => "The file format is not supported.",
            ErrorCode.FileReadError => "An error occurred while reading the file.",
            ErrorCode.FileWriteError => "An error occurred while writing to the file.",
            ErrorCode.FileUploadFailed => "The file upload failed.",
            ErrorCode.FileSizeExceeded => "The file size exceeds the allowable limit.",
            ErrorCode.FileCorrupted => "The file is corrupted.",
            ErrorCode.FileAlreadyExists => "A file with the same name already exists.",
            ErrorCode.FileDownloadFailed => "The file download failed.",
            ErrorCode.FileDeletionFailed => "The file deletion failed.",
            ErrorCode.FileLockError => "The file is locked and cannot be accessed.",
            ErrorCode.FilePermissionError => "Insufficient permissions to access the file.",
        
        #endregion

        #region Network Errors
        
            ErrorCode.NetworkUnavailable => "The network is unavailable.",
            ErrorCode.ConnectionTimeout => "The connection has timed out.",
            ErrorCode.ConnectionRefused => "The connection was refused.",
            ErrorCode.NetworkError => "A network error occurred.",
            ErrorCode.HostNotFound => "The host could not be found.",
            ErrorCode.ProtocolError => "A protocol error occurred.",
            ErrorCode.NetServiceUnavailable => "The requested service is unavailable.",
            ErrorCode.InvalidRequest => "The request is invalid.",
            ErrorCode.NetworkConfigurationError => "There is a network configuration error.",
            ErrorCode.DNSResolutionFailed => "DNS resolution failed.",
            ErrorCode.SSLHandshakeFailed => "The SSL handshake failed.",
            ErrorCode.ConnectionReset => "The connection was reset.",
            
      
            ErrorCode.EncryptionFailed => "Encryption failed.",
            ErrorCode.DecryptionFailed => "Decryption failed.",
            ErrorCode.SecurityViolation => "A security violation has occurred.",
            ErrorCode.InvalidCertificate => "The certificate is invalid.",
            ErrorCode.UnauthorizedAccessAttempt => "Unauthorized access attempt detected.",
            ErrorCode.DataBreach => "A data breach has been detected.",
            ErrorCode.SecurityProtocolError => "There is an error with the security protocol.",
            ErrorCode.AccessDenied => "Access is denied.",
            ErrorCode.InsufficientSecurity => "Security measures are insufficient.",
            ErrorCode.AuthenticationError => "An authentication error occurred.",
            ErrorCode.UnauthorizedIP => "Access from an unauthorized IP address.",
            ErrorCode.SecurityKeyMismatch => "The security key does not match.",
        #endregion

        #region General Errors
            ErrorCode.GeneralError => "An unexpected error occurred.",
            ErrorCode.OperationFailed => "The operation failed.",
            ErrorCode.UnexpectedError => "An unexpected error occurred.",
            ErrorCode.UnknownError => "An unknown error occurred.",
            ErrorCode.FeatureNotImplemented => "The requested feature is not yet implemented.",
            ErrorCode.DeprecatedFeature => "The feature is deprecated and no longer supported.",
            ErrorCode.ServiceNotAvailable => "The service is not available at this time.",
            ErrorCode.TimeoutError => "The operation timed out.",
            ErrorCode.ResourceUnavailable => "The requested resource is unavailable.",
            ErrorCode.RequestCanceled => "The request was canceled.",


            _ => throw new ArgumentOutOfRangeException(nameof(code), code, null)
        };
        #endregion
    }
}
