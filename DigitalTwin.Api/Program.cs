using DigitalTwin.Api.Middlewares;
using DigitalTwin.Common.AppsettingsModels;
using DigitalTwin.Common.Constants;
using DigitalTwin.Common.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var configuration = builder.Configuration;
configuration.AddJsonFile("appsettings.json", true)
    .AddJsonFile($"appsettings.{env}.json", true, true);

// Add services to the container.
builder.Services.AutoRegisterServices(builder.Configuration,
    typeof(Program).Assembly,
    typeof(DigitalTwin.Business.Assembly).Assembly,
    typeof(DigitalTwin.Common.Assembly).Assembly,
    typeof(DigitalTwin.Models.Assembly).Assembly,
    typeof(DigitalTwin.Data.Assembly).Assembly);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Azure AD
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
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

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(ApplicationDomain.ApplicationName, new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build());
});

var app = builder.Build();

app.UseCors(Appsettings.AllowSpecificOrigins);

// Configure the HTTP request pipeline.
if (!app.Environment.IsProduction())
{
    // Enable middleware to serve generated Swagger as a JSON endpoint.
    app.UseSwagger();

    // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
    // specifying the Swagger JSON endpoint.
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Digital Twin v1"); });
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseUrlRewriter(builder.Configuration);

app.UseDefaultFiles();

app.UseStaticFiles();

app.UseMiddleware<HeaderMiddleware>();

app.UseMiddleware<LoggerMiddleware>();

app.UseCustomExceptionHandler(app.Environment);

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseFluentValidationExceptionHandler();

if (app.Environment.IsDevelopment())
    app.MapControllers().AllowAnonymous();
else
    app.MapControllers();

app.Run();