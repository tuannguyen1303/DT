using DigitalTwin.Common.AppsettingsModels;
using DigitalTwin.Common.Constants;
using DigitalTwin.Common.Installer;
using DigitalTwin.Models;

namespace DigitalTwin.Api.Installer
{
    public class CorsInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(Appsettings.AllowSpecificOrigins,
                    builders =>
                    {
                        var cors = configuration.GetSection(Appsettings.CORS).Get<CorsConfig>();
                        if (cors.Urls != null)
                            builders.WithOrigins(cors!.Urls).AllowAnyHeader().AllowAnyMethod();
                    });
            });
        }
    }
}