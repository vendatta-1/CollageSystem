using System.ComponentModel.DataAnnotations;
using CollageSystem.Core.Interfaces.Data;


namespace CollageSystem.Utilities.Helpers.CustomAttributes;

public class UniqueAttribute<TModel>(string propertyName) : ValidationAttribute where TModel : class
{
    private readonly string _propertyName = propertyName;

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        try
        {
            if (value == null)
                return new ValidationResult("value is null");

            IApplicationDbContext? dbContext = (IApplicationDbContext)validationContext.GetService(typeof(IApplicationDbContext)) ?? null;

            if (dbContext == null)
                throw new NullReferenceException("dbContext is null");

            var propInfo = typeof(TModel).GetProperty(_propertyName);

            if (propInfo == null)
                return new ValidationResult($"Property {_propertyName} not found in type {typeof(TModel).Name}");

            var result = dbContext.Set<TModel>().AsEnumerable().Any(x => propInfo.GetValue(x, null)?.Equals(value));

            return result is true ? new ValidationResult("already exists in the database") : ValidationResult.Success;
        }
        catch (Exception e)
        {
            throw;
        }
        
    }
}