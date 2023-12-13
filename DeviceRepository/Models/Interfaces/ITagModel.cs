namespace DeviceRepository.Models.Interfaces;

public interface ITagModel : IModelAdditionalInfo
{
    string? Name { get; }
}