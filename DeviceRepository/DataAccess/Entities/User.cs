namespace DeviceRepository.DataAccess.Entities;

internal class User : RepositoryEntityWithAdditionalInfo
{
    public required string Login { get; set; }
    public string? Name { get; set; }

    public List<Device> Devices { get; } = [];
    public List<Tag> Tags { get; } = [];
}
