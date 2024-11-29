namespace CollageSystem.Core.Validation;
public enum FailureLevel
{
    Critical,   // Operation cannot proceed; it’s essential to handle this error to continue.
    Important,  // Significant error, but the operation might proceed; requires attention.
    Minor       // Minor issue that does not impact the overall operation significantly.
}