using System.Diagnostics.Metrics;

namespace AuthenticationService.Metrics;

public interface IAuthenticationMetrics
{
    void LoggedIn();
    void SignedUp();
    void RefreshedToken();
}

public class AuthenticationMetrics : IAuthenticationMetrics
{
    //Books meters
    private Counter<int> LoginCounter { get; }
    private Counter<int> SignUpCounter { get; }
    private Counter<int> TokenRefreshCounter { get; }

    public AuthenticationMetrics(IMeterFactory meterFactory, IConfiguration configuration)
    {
        var meter = meterFactory.Create("AuthenticationService");

        LoginCounter = meter.CreateCounter<int>("login");
        SignUpCounter = meter.CreateCounter<int>("sign-up");
        TokenRefreshCounter = meter.CreateCounter<int>("token-refresh");
    }

    //Books meters
    public void LoggedIn() => LoginCounter.Add(1);
    public void SignedUp() => SignUpCounter.Add(1);
    public void RefreshedToken() => TokenRefreshCounter.Add(1);
}
