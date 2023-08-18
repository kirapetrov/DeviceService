namespace DeviceRepository.Entities;

internal class Device : RepositoryEntity
{
    public string? Name { get; set; }
    public string? IpAddress { get; set; }
}
