using TokenService.Settings;
using Service.Base.Setup;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.AspNetCore.Authentication.Certificate;
using System.Security.Claims;
using TokenService.Services;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.ConfigureHttpsDefaults(options =>
    {
        options.ClientCertificateMode = ClientCertificateMode.RequireCertificate;
        options.AllowAnyClientCertificate();
    });
});

builder.Services.AddOpenTelemetrySettings();

builder.Services.AddOptions<JwtSettings>()
    .BindConfiguration("JwtSettings")
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddOptions<CustomCertificateSettings>()
    .BindConfiguration("CustomCertificate")
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddGrpc();
builder.Services.AddScoped<ICertificateValidationService, CertificateValidationService>();

builder.Services.AddLogging(conf =>
{
    conf.AddOpenTelemetry(otel => { otel.AddDefaultLogging(builder); });
});

builder.Services.AddOpenTelemetry()
    .AddCurrentServiceAsResource(builder.GetApplicationName())
    .AddTracing(conf =>
    {
        conf.AddDefaultTracing(builder);
    })
    .AddMetrics(conf =>
    {
        conf.AddDefaultMetrics(builder);
    });

builder.Services
    .AddAuthentication()
    .AddCertificate(options =>
    {
        options.AllowedCertificateTypes = CertificateTypes.All;
        options.RevocationMode = X509RevocationMode.NoCheck;
        options.ValidateValidityPeriod = true;
        
        options.Events = new CertificateAuthenticationEvents
        {
            OnCertificateValidated = context =>
            {
                var validationService = context.HttpContext.RequestServices.GetRequiredService<ICertificateValidationService>();

                if (validationService.ValidateCertificate(context.ClientCertificate))
                {
                    context.Success();
                }
                else
                {
                    context.Fail("Invalid client certificate");
                }

                return Task.CompletedTask;
            }
        };
    });

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.ConfigureHttpsDefaults(options =>
        options.ClientCertificateMode = ClientCertificateMode.RequireCertificate);
});

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapGrpcService<TokenService.Grpc.TokenGrpcService>().RequireAuthorization();

app.Run();