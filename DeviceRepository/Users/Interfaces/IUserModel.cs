using DeviceRepository.Common.Models.Interfaces;

namespace DeviceRepository.Users.Interfaces;

public interface IUserModel : IModelAdditionalInfo
{
    string Login { get; }
    string? Name { get; }
}