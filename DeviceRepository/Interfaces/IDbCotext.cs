using DeviceRepository.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DeviceRepository.Interfaces;

internal interface IDbContext : IDisposable
{
    public DbSet<Device> Devices { get; }
    
    ValueTask<EntityEntry<Device>> AddAsync(Device device);
    EntityEntry<Device> Remove(Device device);
    Task<int> SaveChangesAsync();
}