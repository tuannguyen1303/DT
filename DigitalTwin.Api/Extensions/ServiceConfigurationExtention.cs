using DigitalTwin.Common.AppsettingsModels;
using DigitalTwin.Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace DigitalTwin.Api.Extensions;

public static class ServiceConfigurationExtention
{
    public static void AuthenticationBuilder(this WebApplicationBuilder builder, string schema)
    {
        builder.Services.AddAuthentication(schema).AddJwtBearer(options =>
        {
            var azureAd = builder.Configuration.GetSection(Appsettings.AzureAD).Get<AzureADConfig>();
            options.Authority = azureAd.Authority;
            options.Audience = azureAd.Audience;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = false,
                ValidateLifetime = false,
                ValidateActor = false,
                ValidateTokenReplay = false
            };
        });
    }

    public static void AuthorizationService(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(ApplicationDomain.ApplicationName, new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build());
        });
    }
}