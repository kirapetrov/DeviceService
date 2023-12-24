using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

using System.Runtime.CompilerServices;
using DeviceRepository.DataAccess;
using DeviceRepository.Devices.Interfaces;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace DeviceRepository;

public static class DeviceRepositoryDependencyInjector
{
    public static void InjectDeviceRepository(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DeviceContext>(options => 
            options.UseSqlServer(configuration.GetConnectionString("DevicesConnectionString")));
        services.AddScoped<IDeviceRepository, Devices.DeviceRepository>();
    }
}