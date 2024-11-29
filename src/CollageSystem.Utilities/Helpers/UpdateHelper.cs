using System.Reflection;

namespace CollageSystem.Utilities.Helpers;

public static class UpdateHelper
{
    /// <summary>
    /// Updates the properties of the destination object with non-default values from the source object.
    /// </summary>
    /// <typeparam name="T">The type of the destination object.</typeparam>
    /// <typeparam name="TDto">The type of the source object.</typeparam>
    /// <param name="dest">The destination object to be updated.</param>
    /// <param name="source">The source object from which to copy non-default values.</param>
    /// <returns>The updated destination object.</returns>
    public static T Update<T, TDto>(T dest, TDto source)
    {
        var destProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var sourceProperties = typeof(TDto).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var sourceProperty in sourceProperties)
        {
            var destProperty = destProperties.FirstOrDefault(p => p.Name == sourceProperty.Name);
            if (destProperty != null && destProperty.CanWrite)
            {
                var sourceValue = sourceProperty.GetValue(source, null);

                // Check if the value is not null and not the default value for value types
                if (sourceValue != null && !IsDefaultValue(sourceValue))
                {
                    destProperty.SetValue(dest, sourceValue);
                }
            }
        }

        return dest;
    }

    private static bool IsDefaultValue(object value)
    {
        var type = value.GetType();
        if (type == typeof(string))
            return string.IsNullOrEmpty((string)value) || value.ToString() == "string";

        return type.IsValueType && Activator.CreateInstance(type).Equals(value);
    }
}