namespace DeviceRepository.DataAccess.Entities;

internal class RepositoryEntityWithAdditionalInfo : RepositoryEntity
{    
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}