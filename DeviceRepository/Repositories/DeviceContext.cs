using DeviceRepository.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeviceRepository.Repositories;

internal class DeviceContext : DbContext
{
    internal DeviceContext() { }

    public DeviceContext(DbContextOptions<DeviceContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Device>? Devices { get; set; }
}