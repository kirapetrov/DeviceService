namespace DeviceRepository.Models.Interfaces;

public abstract class ModelWithAdditionalInfo : ModelBase, IModelAdditionalInfo
{
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
} 