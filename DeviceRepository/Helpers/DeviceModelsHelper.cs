using DeviceRepository.Entities;
using DeviceRepository.Models.Interfaces;
using DeviceRepository.Models;

namespace DeviceRepository.Helpers;

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

    public static IUserModel GetModel(this User user)
    {
        var userModel = new UserModel(user.Login)
        {
            Name = user.Name
        };

        userModel.AppendAdditionalInfo(user);
        return userModel;
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

    public static ITagModel GetModel(this Tag tag, bool withLinks = false)
    {
        var tagModel = new TagModel
        {
            Name = tag.Name
        };

        if (withLinks)
        {
            tagModel.Devices =
                tag.Devices.Select(x => x.GetModel()).ToArray();
        }

        tagModel.AppendAdditionalInfo(tag);
        return tagModel;
    }

    private static void AppendAdditionalInfo(
        this ModelWithAdditionalInfo additionalInfo,
        RepositoryEntityWithAdditionalInfo repositoryAdditionlInfo)
    {
        additionalInfo.AppendRepositoryEntityInfo(repositoryAdditionlInfo);

        additionalInfo.CreatedAt = repositoryAdditionlInfo.CreatedAt;
        additionalInfo.UpdatedAt = repositoryAdditionlInfo.UpdatedAt;
    }

    private static void AppendRepositoryEntityInfo(
        this ModelBase modelBase,
        RepositoryEntity repositoryEntity)
    {
        modelBase.Identifier = repositoryEntity.Id;
    }
}