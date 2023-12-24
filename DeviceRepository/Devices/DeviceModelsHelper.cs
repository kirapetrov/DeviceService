using DeviceRepository.Common.Models;
using DeviceRepository.DataAccess.Entities;
using DeviceRepository.Devices.Interfaces;
using DeviceRepository.Tags;

namespace DeviceRepository.Devices;

internal static class DeviceModelsHelper
{
    public static Device? GetEntity(
        this IModifyDeviceModel deviceModel,
        long userId)
    {
        if (deviceModel == null)
        {
            return null;
        }

        return new Device
        {
            UserId = userId,
            Name = deviceModel.Name,
            IpAddress = deviceModel.IpAddress,
            CreatedAt = DateTimeOffset.UtcNow
        };
    }

    public static IDeviceModel GetModel(this Device device, bool withLinks = false)
    {
        var deviceModel = new DeviceModel
        {
            Name = device.Name,
            IpAddress = device.IpAddress
        };

        if (withLinks)
        {
            deviceModel.Tags =
                device.Tags.Select(x => x.GetModel()).ToArray();
        }

        deviceModel.AppendAdditionalInfo(device);
        return deviceModel;
    }
}