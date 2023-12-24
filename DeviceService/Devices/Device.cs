using DeviceService.Common.Models;
using DeviceService.Tags;

namespace DeviceService.Devices;

public record Device(
    long Identifier,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt,
    string? Name,
    string? IpAddress,
    IReadOnlyCollection<Tag> Tags
): ModelBase(
    Identifier,
    CreatedAt,
    UpdatedAt
);