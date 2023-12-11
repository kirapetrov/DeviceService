namespace DeviceRepository.Models.Interfaces;

public interface IModelAdditionalInfo : IModelBase
{
    DateTimeOffset CreatedAt { get; set; }
    DateTimeOffset? UpdatedAt { get; set; }
}