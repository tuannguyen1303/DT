using System.Text.Json.Serialization;
using DigitalTwin.Common.Installer;
using DigitalTwin.Common.JsonConverters;

namespace DigitalTwin.Api.Installer
{
    public class ApiInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers()
                .AddJsonOptions(configure =>
                {
                    configure.JsonSerializerOptions.Converters.Add(new DateTimeZeroTimeZone());
                    configure.JsonSerializerOptions.Converters.Add(new DateTimeNullableZeroTimeZone());
                    configure.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });
            services.AddHttpContextAccessor();
        }
    }
}
