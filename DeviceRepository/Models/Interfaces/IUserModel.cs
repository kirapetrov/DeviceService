namespace DeviceRepository.Models.Interfaces;

public interface IUserModel : IModelAdditionalInfo
{
    string Login { get; }
    string? Name { get; }
}