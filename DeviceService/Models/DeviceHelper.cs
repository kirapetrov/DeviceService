using DeviceRepository.Models;
using DeviceRepository.Models.Interfaces;

namespace DeviceService.Models;

public static class DeviceHelper
{
    public static IDeviceModel ToModel(this Device device)
    {
        return new DeviceModel
        {
            Identifier = device.Identifier,
            Name = device.Name,
            IpAddress = device.IpAddress
        };
    }

    public static Device ToDevice(this IDeviceModel deviceModel)
    {
        return new Device(
            deviceModel.Identifier,
            deviceModel.Name,
            deviceModel.IpAddress
        );
    }
}