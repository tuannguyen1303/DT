using DigitalTwin.Common.Constants;
using DigitalTwin.Common.Installer;
using DigitalTwin.Data.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DigitalTwin.Data.Installers
{
    public class DbInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddDbContext<DigitalTwinContext>((opt) =>
                {
                    opt.UseNpgsql(configuration.GetConnectionString(Appsettings.DefaultConnection),
                        opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds));
                });
        }
    }
}