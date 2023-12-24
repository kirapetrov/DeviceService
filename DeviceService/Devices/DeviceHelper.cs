using DeviceRepository.Devices;
using DeviceRepository.Devices.Interfaces;
using DeviceRepository.Tags.Interfaces;
using DeviceService.Tags;

namespace DeviceService.Devices;

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

    public static Device ToDevice(
        this IDeviceModel deviceModel,
        bool withLinks = false)
    {
        var tags = withLinks
            ? deviceModel.Tags.Select(x => x.ToTag()).ToArray()
            : [];

        return new Device(
            deviceModel.Identifier,
            deviceModel.CreatedAt,
            deviceModel.UpdatedAt,
            deviceModel.Name,
            deviceModel.IpAddress,
            tags
        );
    }

    public static Tag ToTag(
        this ITagModel tagModel,
        bool withLinks = false)
    {
        var devices = withLinks
            ? tagModel.Devices.Select(x => x.ToDevice()).ToArray()
            : [];

        return new Tag(
            tagModel.Identifier,
            tagModel.CreatedAt,
            tagModel.UpdatedAt,
            tagModel.Name,
            devices
        );
    }
}