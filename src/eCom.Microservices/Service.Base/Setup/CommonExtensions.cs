using Microsoft.Extensions.Hosting;

namespace Service.Base.Setup;

public static class CommonExtensions
{
    public static string GetApplicationName(this IHostApplicationBuilder hostApplication) => hostApplication.Environment.ApplicationName;
}
