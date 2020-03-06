using System.Security.Cryptography.X509Certificates;

namespace Microsoft.Pfe.Samples.AzureFunctions.Cds.Auth.Services
{
    public interface ICertificateService
    {
        X509Certificate2 GetCertificate();
    }
}
