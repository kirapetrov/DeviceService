using DeviceRepository.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DeviceRepository;

internal class DeviceContextFactory : IDesignTimeDbContextFactory<DeviceContext>
{
    public DeviceContext CreateDbContext(string[] args)
    {
        var dbContextBuilder = new DbContextOptionsBuilder<DeviceContext>();
        var connString = "Server=.\\;Database=DevicesDb;TrustServerCertificate=True;Integrated Security=SSPI;";
        dbContextBuilder.UseSqlServer(connString);
        return new DeviceContext(dbContextBuilder.Options);
    }
}