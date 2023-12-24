using System.Reflection;
using DeviceRepository.Common.Search;
using Microsoft.EntityFrameworkCore;

namespace DeviceRepository.Helpers;

public static class QueryHelper
{
    public static IQueryable<T> AppendOrder<T>(
        this IQueryable<T> query,
        string? propertyName,
        OrderType orderType = OrderType.Ascending)
    {
        if (!string.IsNullOrWhiteSpace(propertyName) &&
            HasProperty<T>(propertyName))
        {
#pragma warning disable CS8604 // Possible null reference argument.
            return orderType == OrderType.Descending
                ? query.OrderByDescending(x => EF.Property<object>(x, propertyName))
                : query.OrderBy(x => EF.Property<object>(x, propertyName));
#pragma warning restore CS8604 // Possible null reference argument.
        }

        return query;        
    }

    public static IQueryable<T> AppendPage<T>(
        this IQueryable<T> query,
        ushort number,
        ushort size)
    {
        if (number > 0 && size > 0)
        {
            return query
                .Skip((number - 1) * size)
                .Take(size);
        }

        return query;
    }

    public static IQueryable<T> AppendParameters<T>(
        this IQueryable<T> query,
        IReadOnlyCollection<SearchParameters>? searchParameters)
    {
        if(searchParameters?.Any() != true)
        {
            return query;
        }

        foreach (var searchParameter in searchParameters)
        {
            query = query.AppendParameter(searchParameter);
        }

        return query;
    }

    public static IQueryable<T> AppendParameter<T>(
        this IQueryable<T> query,
        SearchParameters searchParameter)
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
#pragma warning disable CS8604 // Possible null reference argument.
                return query.Where(x => EF.Property<object>(x, propertyName) == null);
#pragma warning restore CS8604 // Possible null reference argument.
            }

            if (searchParameter.Operand == OperandType.NotEmpty)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                return query.Where(x => EF.Property<object>(x, propertyName) != null);
#pragma warning restore CS8604 // Possible null reference argument.
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
#pragma warning disable CS8604 // Possible null reference argument.
        return operand switch
        {
            OperandType.Contains => query.Where(x => EF.Property<string>(x, propertyName).Contains(stringValue)),
            OperandType.StartWith => query.Where(x => EF.Property<string>(x, propertyName).StartsWith(stringValue)),
            OperandType.EndWith => query.Where(x => EF.Property<string>(x, propertyName).EndsWith(stringValue)),
            OperandType.Equals => query.Where(x => EF.Property<string>(x, propertyName) == stringValue),
            OperandType.NotEquals => query.Where(x => EF.Property<string>(x, propertyName) != stringValue),
            _ => throw new ArgumentException($"Unknown {nameof(OperandType)}")
        };
#pragma warning restore CS8604 // Possible null reference argument.
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