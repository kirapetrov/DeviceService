using DeviceRepository.Models.Interfaces;

namespace DeviceRepository.Models;

public class UserModel(string login) : ModelWithAdditionalInfo, IUserModel
{
    public string Login { get; set; } = login;

    public string? Name { get; set; }

    public List<IDeviceModel> Devices { get; } = [];
}