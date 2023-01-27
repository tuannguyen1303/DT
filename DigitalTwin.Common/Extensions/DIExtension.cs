using DigitalTwin.Common.Installer;
using DigitalTwin.Common.Installer.Attributes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DigitalTwin.Common.Extensions
{
    public static class DIExtension
    {
        public static void AutoRegisterServices(this IServiceCollection services,
            IConfiguration configuration, params System.Reflection.Assembly[] assemblies)
        {
            var installers = assemblies
                .SelectMany(asb => asb.ExportedTypes)
                .Where(exp => typeof(IInstaller).IsAssignableFrom(exp) &&
                              !exp.IsInterface &&
                              !exp.IsAbstract &&
                              !exp.GetCustomAttributes(typeof(NotRunInstallerAttribute), false).Any())
                .Select(Activator.CreateInstance)
                .Cast<IInstaller>()
                .ToList();

            installers.ForEach(installer => installer.InstallServices(services, configuration));
        }
    }
}