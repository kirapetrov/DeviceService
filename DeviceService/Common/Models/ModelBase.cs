namespace DeviceService.Common.Models;

public abstract record ModelBase(
    long Identifier,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt); 