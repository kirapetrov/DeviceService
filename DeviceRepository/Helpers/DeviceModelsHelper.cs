using DeviceRepository.Entities;
using DeviceRepository.Models.Interfaces;
using DeviceRepository.Models;

namespace DeviceRepository.Helpers;

internal static class DeviceModelsHelper
{
    public static Device? GetEntity(this IDeviceModel deviceModel)
    {
        if(deviceModel == null) {
            return null;
        }

        return GetEntity(
            deviceModel.Name,
            deviceModel.IpAddress,
            deviceModel.Identifier);
    }

    public static Device GetEntity(
        string? name,
        string? ipAddress,
        long identifier = 0)
    {
        return new Device
        {
            Id = identifier,
            Name = name,
            IpAddress = ipAddress
        };
    }

    public static IDeviceModel GetModel(this Device device)
    {
        return new DeviceModel
        {
            Identifier = device.Id,
            Name = device.Name,
            IpAddress = device.IpAddress
        };
    }
}