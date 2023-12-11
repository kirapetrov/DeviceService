namespace DeviceRepository.Entities;

internal class Device : RepositoryEntityWithAdditionalInfo
{
    public string? Name { get; set; }
    public string? IpAddress { get; set; }

    public long UserId { get; set; }
    public User User { get; set; }
    public List<Tag> Tags { get; } = [];
}