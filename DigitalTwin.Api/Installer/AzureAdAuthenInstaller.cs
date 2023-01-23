using DigitalTwin.Common.Installer;

namespace DigitalTwin.Api.Installer
{
    public class AzureAdAuthenInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            //var applicationSecurity = configuration.GetSection(Appsettings.ApplicationSecurity).Get<ApplicationSecurity>();

            //// Authentication
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddMicrosoftIdentityWebApi(configuration)
            //        .EnableTokenAcquisitionToCallDownstreamApi()
            //            .AddMicrosoftGraph(configuration.GetSection(Appsettings.MSGraph))
            //            .AddInMemoryTokenCaches();

            //// Autorization
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy(ApplicationDomain.ApplicationName, policy =>
            //    {
            //        policy.RequireClaim(ClaimConstants.Scope,
            //            applicationSecurity != null ?
            //            applicationSecurity.ApplicationAuthorization.AllowedScopes :
            //            System.Array.Empty<string>());
            //    });
            //});
        }
    }
}
