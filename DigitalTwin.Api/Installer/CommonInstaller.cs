using DigitalTwin.Common.Installer;

namespace DigitalTwin.Api.Installer
{
    public class CommonInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient();
            services.AddLogging(config =>
            {
                config.SetMinimumLevel(LogLevel.Debug);
            });
        }
    }
}
