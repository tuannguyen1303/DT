using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DigitalTwin.Common.Installer
{
    /// <summary>
    /// Impelement this Interface will auto discover and register your service into DI.
    /// </summary>
    public interface IInstaller
    {
        /// <summary>
        /// Register your services here.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        void InstallServices(IServiceCollection services, IConfiguration configuration);
    }
}