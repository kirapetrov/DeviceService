using DeviceRepository.Common.Models;
using DeviceRepository.Devices.Interfaces;
using DeviceRepository.Users.Interfaces;

namespace DeviceRepository.Users;

public class UserModel(string login) : ModelWithAdditionalInfo, IUserModel
{
    public string Login { get; set; } = login;

    public string? Name { get; set; }

    public List<IDeviceModel> Devices { get; } = [];
}