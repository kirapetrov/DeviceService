namespace DeviceRepository.Entities;

internal class Tag : RepositoryEntityWithAdditionalInfo
{
    public string? Name { get; set; }

    public long UserId { get; set; }
    public User User { get; set; }
    public List<Device> Devices { get; } = [];
}