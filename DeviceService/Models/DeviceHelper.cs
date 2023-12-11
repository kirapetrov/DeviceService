using DeviceRepository.Models;
using DeviceRepository.Models.Interfaces;

namespace DeviceService.Models;

public static class DeviceHelper
{
    public static IModifyDeviceModel ToModel(this Device device)
    {
        return new ModifyDeviceModel
        {
            Name = device.Name,
            IpAddress = device.IpAddress
        };
    }

    public static Device ToDevice(this IDeviceModel deviceModel)
    {
        return new Device(
            deviceModel.Identifier,
            deviceModel.Name,
            deviceModel.IpAddress,
            deviceModel.CreatedAt,
            deviceModel.UpdatedAt
        );
    }
}