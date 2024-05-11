using Microsoft.Extensions.Options;
using System.Security.Cryptography.X509Certificates;
using TokenService.Settings;

namespace TokenService.Services;

public interface ICertificateValidationService
{
    public bool ValidateCertificate(X509Certificate2 clientCertificate);
}

public class CertificateValidationService : ICertificateValidationService
{
    private readonly IOptions<CustomCertificateSettings> _options;

    public CertificateValidationService(IOptions<CustomCertificateSettings> options)
    {
        this._options = options;
    }

    public bool ValidateCertificate(X509Certificate2 clientCertificate)
    {
        var caCert = new X509Certificate2(_options.Value.CertificatePath);

        X509Chain chain = new X509Chain();
        chain.ChainPolicy = new X509ChainPolicy()
        {
            RevocationMode = X509RevocationMode.NoCheck,
            VerificationFlags = X509VerificationFlags.NoFlag
        };
        chain.ChainPolicy.ExtraStore.Add(caCert);

        bool isValid = chain.Build(clientCertificate);
        if (isValid && chain.ChainElements.Count > 1 && chain.ChainElements[1].Certificate.Thumbprint == caCert.Thumbprint)
        {
            return true;
        }

        return false;
    }
}