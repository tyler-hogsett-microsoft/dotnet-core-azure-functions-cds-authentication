
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Pfe.Samples.AzureFunctions.Cds.Auth.Services;

namespace Microsoft.Pfe.Samples.AzureFunctions.Cds.Auth.Components
{
    public class LinuxFunctionAppCertificateService : ICertificateService
    {
        private readonly string CertificateThumbprint;

        public LinuxFunctionAppCertificateService(IOptions<Options> options) {
            CertificateThumbprint = options.Value.CertificateThumbprint;
        }

        public LinuxFunctionAppCertificateService(string certificateThumbprint) {
            CertificateThumbprint = certificateThumbprint;
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
                $"{CertificateDirectory}/{CertificateThumbprint}.p12");
            return service;
        }
    }
}