using GatewayService;
using Service.Base.Setup;
using System.Reflection.Metadata.Ecma335;
using TokenService.Specification.ServiceConsumer;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddOpenTelemetrySettings();

builder.Services.AddTokenServiceForConsumer();

builder.Services.AddLogging(conf =>
{
    conf.AddConsole();
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

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.UseHttpsRedirection();

app.UseMiddleware<JwtAuthenticationMiddleware>();
app.MapReverseProxy();

app.MapGet("/test", () => Results.Ok());

app.Run();