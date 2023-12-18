namespace DeviceService.Models;

public abstract record ModelBase(
    long Identifier,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt); 