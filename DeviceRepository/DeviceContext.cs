using DeviceRepository.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeviceRepository;

internal class DeviceContext : DbContext
{
    public DeviceContext(DbContextOptions<DeviceContext> options) 
        : base(options)
    {
    }

    public virtual DbSet<Device> Devices { get; set; }
}