using System.Linq.Expressions;
using AutoMapper;

namespace CollageSystem.Utilities.Helpers;

public static class AutoMapperExtensions
{
    public static Expression<Func<TDestination, object>> MapExpression<TSource, TDestination>(
        this IMapper mapper,
        Expression<Func<TSource, object>> sourceExpression)
    {
        return mapper.Map<Expression<Func<TDestination, object>>>(sourceExpression);
    }
}