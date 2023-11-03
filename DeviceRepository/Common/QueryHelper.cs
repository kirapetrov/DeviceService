using System.Reflection;
using DeviceRepository.Common.Order;
using DeviceRepository.Common.Page;
using DeviceRepository.Common.Search;
using Microsoft.EntityFrameworkCore;

namespace DeviceRepository.Common;

public static class QueryHelper
{
    public static IQueryable<T> AppendOrder<T>(this IQueryable<T> query, OrderInfo orderInfo)
    {
        if (string.IsNullOrWhiteSpace(orderInfo.Name) ||
            !HasProperty<T>(orderInfo.Name))
        {
            return query;
        }

        return orderInfo.OrderType == OrderType.Descending
            ? query.OrderByDescending(x => EF.Property<object>(x, orderInfo.Name))
            : query.OrderBy(x => EF.Property<object>(x, orderInfo.Name));
    }

    public static IQueryable<T> AppendPage<T>(this IQueryable<T> query, PageInfo pageInfo)
    {
        if (pageInfo?.Page > 0 && pageInfo?.Size > 0)
        {
            return query
                .Skip((pageInfo.Page - 1) * pageInfo.Size)
                .Take(pageInfo.Size);
        }

        return query;
    }

    public static IQueryable<T> AppendParameters<T>(
        this IQueryable<T> query,
        IReadOnlyCollection<SearchParameter> searchParameters)
    {
        foreach (var searchParameter in searchParameters)
        {
            query = query.AppendParameter(searchParameter);
        }

        return query;
    }

    public static IQueryable<T> AppendParameter<T>(
        this IQueryable<T> query,
        SearchParameter searchParameter)
    {
        if (searchParameter is null)
        {
            return query;
        }

        var propertyName = searchParameter.Name;
        var propertyType = GetPropertyType<T>(propertyName);
        if (!string.IsNullOrWhiteSpace(propertyName) &&
            propertyType is not null)
        {
            if (searchParameter.Operand == OperandType.Empty)
            {
                return query.Where(x => EF.Property<object>(x, propertyName) == null);
            }

            if (searchParameter.Operand == OperandType.NotEmpty)
            {
                return query.Where(x => EF.Property<object>(x, propertyName) != null);
            }

            if (searchParameter.Value is not null)
            {
                return query.GetString(
                    propertyName,
                    searchParameter.Value,
                    searchParameter.Operand);
            }
        }

        return query;
    }

    private static IQueryable<T> GetString<T>(
        this IQueryable<T> query,
        string propertyName,
        object value,
        OperandType operand)
    {
        var stringValue = (value?.ToString()) ?? throw new ArgumentException($"Parameter {nameof(value)} is null");
        return operand switch
        {
            OperandType.Contains => query.Where(x => EF.Property<string>(x, propertyName).Contains(stringValue)),
            OperandType.StartWith => query.Where(x => EF.Property<string>(x, propertyName).StartsWith(stringValue)),
            OperandType.EndWith => query.Where(x => EF.Property<string>(x, propertyName).EndsWith(stringValue)),
            OperandType.Equals => query.Where(x => EF.Property<string>(x, propertyName) == stringValue),
            OperandType.NotEquals => query.Where(x => EF.Property<string>(x, propertyName) != stringValue),
            _ => throw new ArgumentException($"Unknown {nameof(OperandType)}")
        };
    }

    private static bool HasProperty<T>(string propertyName) =>
        GetProperty<T>(propertyName) is not null;

    private static Type? GetPropertyType<T>(string? propertyName)
    {
        if (!string.IsNullOrWhiteSpace(propertyName))
        {
            var property = GetProperty<T>(propertyName);
            if (property is not null)
            {
                return property.DeclaringType;
            }
        }

        return null;
    }

    private static PropertyInfo? GetProperty<T>(string propertyName) =>
        typeof(T).GetProperties().FirstOrDefault(x => string.CompareOrdinal(x.Name, propertyName) == 0);
}