using DeviceRepository.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeviceRepository.DataAccess;

internal class DeviceContext : DbContext
{
    internal DeviceContext() { }

    public DeviceContext(DbContextOptions<DeviceContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User>? Users { get; set; }

    public virtual DbSet<Device>? Devices { get; set; }

    public virtual DbSet<Tag>? Tags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Device>()
            .HasMany(x => x.Tags)
            .WithMany(y => y.Devices)
            .UsingEntity(
                x => x.HasOne(typeof(Tag)).WithMany().OnDelete(DeleteBehavior.Restrict),
                y => y.HasOne(typeof(Device)).WithMany().OnDelete(DeleteBehavior.Restrict));
    }
}