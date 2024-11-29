using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Linq.Dynamic.Core;

namespace CollageSystem.Utilities.Helpers
{
    public static class ExpressionHelper
    {
        private const string OrStart = "or::";
        private const string OrEnd = "::or";
        private const string AndStart = "and::";
        private const string AndEnd = "::and";
        private const string Delimiter = ",";

        /// <summary>
        /// Parses a complex query into a single LINQ expression combining 'or' and 'and' conditions.
        /// </summary>
        /// <typeparam name="T">The type of the object to filter.</typeparam>
        /// <param name="query">The complex query string.</param>
        /// <returns>An expression that combines 'or' and 'and' conditions.</returns>
        public static Expression<Func<T, bool>> ParseComplexQuery<T>(string query)
        {
            var parsedExpression = DynamicExpressionParser.ParseLambda<T, bool>(ParsingConfig.Default, false, query);
            return (Expression<Func<T, bool>>)parsedExpression;
        }

        /// <summary>
        /// Parses a simple expression string into an expression of type Func<T, bool>.
        /// </summary>
        /// <typeparam name="T">The type of the object to filter.</typeparam>
        /// <param name="filter">The filter string.</param>
        /// <returns>An expression representing the filter.</returns>
        public static Expression<Func<T, bool>> CreateFilterExpression<T>(string filter)
        {
            var operators = new[] { "==", ">=", "<=", ">", "<", "!=" };
            string propertyName = null;
            string op = null;
            string value = null;

            foreach (var opCandidate in operators)
            {
                if (filter.Contains(opCandidate))
                {
                    var parts = filter.Split(new[] { opCandidate }, 2, StringSplitOptions.None);
                    if (parts.Length == 2)
                    {
                        propertyName = parts[0].Trim();
                        op = opCandidate;
                        value = parts[1].Trim();
                        break;
                    }
                }
            }

            if (propertyName == null || op == null || value == null)
            {
                throw new ArgumentException("Invalid filter format", nameof(filter));
            }

            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, propertyName);
            var propertyType = property.Type;

            object convertedValue;
            try
            {
                convertedValue = ConvertToType(value, propertyType);
            }
            catch
            {
                throw new ArgumentException($"Cannot convert filter value to type {propertyType.Name}", nameof(value));
            }

            var constant = Expression.Constant(convertedValue, propertyType);
            Expression comparison = op switch
            {
                "==" => Expression.Equal(property, constant),
                "!=" => Expression.NotEqual(property, constant),
                ">" => Expression.GreaterThan(property, constant),
                ">=" => Expression.GreaterThanOrEqual(property, constant),
                "<" => Expression.LessThan(property, constant),
                "<=" => Expression.LessThanOrEqual(property, constant),
                _ => throw new NotSupportedException($"Operator {op} is not supported")
            };

            return Expression.Lambda<Func<T, bool>>(comparison, parameter);
        }

        /// <summary>
        /// Parses a property name into an expression of type Func<T, object>.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>An expression representing the property.</returns>
        public static Expression<Func<T, object>> ParseExpression<T>(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("Property name cannot be null or empty", nameof(propertyName));
            }

            var parameter = Expression.Parameter(typeof(T), "x");
            var member = Expression.PropertyOrField(parameter, propertyName);
            var convert = Expression.Convert(member, typeof(object));
            return Expression.Lambda<Func<T, object>>(convert, parameter);
        }

        /// <summary>
        /// Converts a string value to the specified type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The target type.</param>
        /// <returns>The converted value.</returns>
        private static object ConvertToType(string value, Type targetType)
        {
            if (targetType == typeof(string))
            {
                return value;
            }
            else if (targetType == typeof(int))
            {
                return int.Parse(value);
            }
            else if (targetType == typeof(long))
            {
                return long.Parse(value);
            }
            else if (targetType == typeof(double))
            {
                return double.Parse(value);
            }
            else if (targetType == typeof(decimal))
            {
                return decimal.Parse(value);
            }
            else if (targetType == typeof(DateTime))
            {
                return DateTime.Parse(value);
            }
            else if (targetType == typeof(bool))
            {
                return bool.Parse(value);
            }
            else
            {
                throw new NotSupportedException($"The type {targetType.Name} is not supported for filtering.");
            }
        }
    }
}