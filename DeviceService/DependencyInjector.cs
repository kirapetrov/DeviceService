using DeviceRepository;

namespace DeviceService;

public static class DependencyInjector
{
    public static void InjectDependencies(this IServiceCollection services)
    {
        services.InjectDeviceRepository();
    }
}