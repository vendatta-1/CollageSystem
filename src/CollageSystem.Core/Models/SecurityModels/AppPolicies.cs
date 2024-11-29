namespace CollageSystem.Core.Models.SecurityModels;

public static class AppPolicies
{
    public const string AdminOnly = "AdminOnly";
    public const string UserOnly = "UserOnly";
    public const string SuperUserOnly = "SuperUserOnly";
    public const string AnyRole = "AnyRole";
}