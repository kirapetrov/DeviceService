using DeviceRepository.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace DeviceRepository;

public static class DependencyInjector
{
    public static void InjectDeviceRepository(this IServiceCollection services)
    {
        services.AddSingleton<IDeviceRepository, DeviceRepository>();
    }
}