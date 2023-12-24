using DeviceService.Common.Models;
using DeviceService.Devices;

namespace DeviceService.Tags;

public record Tag(
    long Identifier,    
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt,
    string? Name,
    IReadOnlyCollection<Device> Devices
): ModelBase(
    Identifier,
    CreatedAt,
    UpdatedAt
);
