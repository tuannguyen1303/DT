using DigitalTwin.Common.Installer;
using DigitalTwin.Models.MediatRPipeline;
using FluentValidation;
using MediatR;

namespace DigitalTwin.Api.Installer
{
    public class MediatRInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(typeof(Business.Assembly).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddValidatorsFromAssembly(typeof(Models.Assembly).Assembly);
        }
    }
}
