using DeviceRepository.Common.Models;
using DeviceRepository.DataAccess.Entities;
using DeviceRepository.Devices;
using DeviceRepository.Tags;
using DeviceRepository.Tags.Interfaces;

namespace DeviceRepository.Tags;

internal static class TagModelsHelper
{
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
}