using DigitalTwin.Common.AppsettingsModels;
using DigitalTwin.Common.Constants;
using DigitalTwin.Common.Installer;

namespace DigitalTwin.Api.Installer
{
    public class AppsettingsInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            // Apply IOptions pattern
            services.Configure<CorsConfig>(configuration
                        .GetSection(Appsettings.CORS));
        }
    }
}
