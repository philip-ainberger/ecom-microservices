using AuthenticationService.Database;
using AuthenticationService.Endpoints;
using AuthenticationService.Metrics;
using AuthenticationService.Services;
using OpenTelemetry.Exporter;
using OpenTelemetry.Trace;
using Service.Base.EntityFrameworkCore.Setup;
using Service.Base.MassTransit.Setup;
using Service.Base.Setup;
using TokenService.Specification.ServiceConsumer;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddOpenTelemetrySettings()
    .AddEntityFrameworkSettings();

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
        conf.AddAspNetCoreInstrumentation();
        conf.AddHttpClientInstrumentation();
        conf.AddSource("Test");

        conf.AddConsoleExporter();

        conf.AddOtlpExporter(otlpOptions =>
        {
            otlpOptions.Endpoint = new Uri("http://otel-collector:4317");
            otlpOptions.Protocol = OtlpExportProtocol.Grpc;
        });

        //conf.AddDefaultTracing(builder);
        //.AddMassTransitTracing();
    });
    //.AddMetrics(conf =>
    //{
    //    conf.AddDefaultMetrics(builder);
    //});

builder.Services.AddDbContextProvider<AuthDbContext>();
builder.Services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
builder.Services.AddSingleton<IAuthenticationMetrics, AuthenticationMetrics>();

builder.Services
    .AddAuthentication()
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.ExpireTimeSpan = TimeSpan.FromHours(12);
        options.LoginPath = string.Empty;
        options.AccessDeniedPath = string.Empty;
        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

//if (app.Environment.IsDevelopment())
//{
//    app.UseDeveloperExceptionPage();

//    using (var scope = app.Services.CreateScope())
//    {
//        var db = scope.ServiceProvider.GetRequiredService<IDbContextProvider<AuthDbContext>>();
//        db.ProvideContext().Database.Migrate();
//    }
//}

//app.UseHttpsRedirection();

app.MapAuthenticationEndpoints();

app.Run();