using TokenService.Settings;
using Service.Base.Setup;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.AspNetCore.Authentication.Certificate;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenTelemetrySettings();

builder.Services.AddOptions<JwtSettings>()
    .BindConfiguration("JwtSettings")
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddGrpc();

builder.Services.AddLogging(conf =>
{
    conf.AddOpenTelemetry(otel => { otel.AddDefaultLogging(builder); });
});

builder.Services.AddOpenTelemetry()
    .AddCurrentServiceAsResource(builder.GetApplicationName())
    .AddTracing(conf =>
    {
        conf.AddDefaultTracing(builder); ;
    })
    .AddMetrics(conf =>
    {
        conf.AddDefaultMetrics(builder);
    });

//builder.Services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme)
//    .AddCertificate(options =>
//    {
//        options.Events = new CertificateAuthenticationEvents
//        {
//            OnCertificateValidated = context =>
//            {
//                var validationService = context.HttpContext.RequestServices
//                    .GetRequiredService<ICertificateValidationService>();

//                if (validationService.ValidateCertificate(context.ClientCertificate))
//                {
//                    var claims = new[]
//                    {
//                        new Claim(
//                            ClaimTypes.NameIdentifier,
//                            context.ClientCertificate.Subject,
//                            ClaimValueTypes.String, context.Options.ClaimsIssuer),
//                        new Claim(
//                            ClaimTypes.Name,
//                            context.ClientCertificate.Subject,
//                            ClaimValueTypes.String, context.Options.ClaimsIssuer)
//                    };

//                    context.Principal = new ClaimsPrincipal(
//                        new ClaimsIdentity(claims, context.Scheme.Name));
//                    context.Success();
//                }

//                return Task.CompletedTask;
//            }
//        };
//    });

//builder.Services.Configure<KestrelServerOptions>(options =>
//{
//    options.ConfigureHttpsDefaults(options =>
//        options.ClientCertificateMode = ClientCertificateMode.RequireCertificate);
//});

var app = builder.Build();

//app.UseAuthentication();
//app.UseAuthorization();

app.MapGrpcService<TokenService.Grpc.TokenGrpcService>();
    //.RequireAuthorization();

app.Run();