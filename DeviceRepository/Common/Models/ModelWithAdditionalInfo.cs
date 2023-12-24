using DeviceRepository.Common.Models.Interfaces;

namespace DeviceRepository.Common.Models;

public abstract class ModelWithAdditionalInfo : ModelBase, IModelAdditionalInfo
{
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}