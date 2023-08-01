using DeviceRepository;

namespace DeviceService;

public static class DependencyInjector
{
    public static void InjectDependencies(this WebApplicationBuilder builder)
    {
        DeviceRepositoryDependencyInjector.InjectDeviceRepository(
            builder.Services,
            builder.Configuration);
    }
}