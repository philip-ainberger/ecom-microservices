using TokenService.Specification.ServiceConsumer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTokenServiceForConsumer();

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.UseHttpsRedirection();

app.MapReverseProxy();

app.Run();