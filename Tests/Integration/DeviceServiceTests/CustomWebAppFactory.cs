using DeviceRepository.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;

using System.Diagnostics;

namespace DeviceServiceTests;

public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var deviceRepository = services.First(
                d => d.ServiceType == typeof(IDeviceRepository));

            services.Remove(deviceRepository);
            // services.AddSingleton<IDeviceRepository>(_ => new DeviceRepositoryMock());

            // services.AddDbContext<ApplicationDbContext>((container, options) =>
            // {
            //     var connection = container.GetRequiredService<DbConnection>();
            //     options.UseSqlite(connection);
            // });
        });

        builder.UseEnvironment("Development");
    }
}