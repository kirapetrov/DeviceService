namespace DeviceService.Models;

public record Device(
    long Identifier,
    string? Name,
    string? IpAddress,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdateAt);