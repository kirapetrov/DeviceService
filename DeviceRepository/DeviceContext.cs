using DeviceRepository.Interfaces;
using DeviceRepository.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DeviceRepository;

internal class DeviceContext : DbContext, IDbContext
{
    public DeviceContext(DbContextOptions<DeviceContext> options) 
        : base(options)
    {
        
    }

    public virtual DbSet<Device> Devices { get; set; }

    public ValueTask<EntityEntry<Device>> AddAsync(Device device) => base.AddAsync(device);

    public EntityEntry<Device> Remove(Device device) => base.Remove(device);
    public Task<int> SaveChangesAsync() => base.SaveChangesAsync();
}