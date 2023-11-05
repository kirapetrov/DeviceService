namespace DeviceRepository.Common;

public record QueryResult<T>(IReadOnlyCollection<T> Collection, int TotalCount);