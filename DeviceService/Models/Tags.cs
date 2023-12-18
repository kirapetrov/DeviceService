namespace DeviceService.Models;

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
