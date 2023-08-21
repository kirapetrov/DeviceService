using DeviceRepository.Entities;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace DeviceRepository;

internal class DeviceContext : DbContext
{
    internal DeviceContext()
    {        
    }

    public DeviceContext(DbContextOptions<DeviceContext> options) 
        : base(options)
    {
    }

    public virtual DbSet<Device> Devices { get; set; }
}