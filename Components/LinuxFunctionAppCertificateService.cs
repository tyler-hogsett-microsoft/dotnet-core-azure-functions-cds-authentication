
using System.Security.Cryptography.X509Certificates;
using Microsoft.Pfe.Samples.AzureFunctions.Cds.Auth.Services;

namespace Microsoft.Pfe.Samples.AzureFunctions.Cds.Auth.Components
{
    public class LinuxFunctionAppCertificateService : ICertificateService
    {
        private readonly string Thumbprint;
        private readonly string Password;

        public LinuxFunctionAppCertificateService(string thumbprint, string password = null) {
            Thumbprint = thumbprint;
            Password = password;
        }

        public X509Certificate2 GetCertificate()
        {
            var service = BuildFileSystemCertificateService();
            return service.GetCertificate();
        }

        private ICertificateService BuildFileSystemCertificateService()
        {
            const string CertificateDirectory = "/var/ssl/private";
            var service = new FileSystemCertificateService(
                $"{CertificateDirectory}/{Thumbprint}.p12",
                Password
            );
            return service;
        }
    }
}
