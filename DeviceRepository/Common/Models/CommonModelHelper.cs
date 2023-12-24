using DeviceRepository.DataAccess.Entities;

namespace DeviceRepository.Common.Models;

internal static class UserModelsHelper
{
    public static void AppendAdditionalInfo(
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