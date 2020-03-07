using System.IO;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Options;
using Microsoft.Pfe.Samples.AzureFunctions.Cds.Auth.Services;

namespace Microsoft.Pfe.Samples.AzureFunctions.Cds.Auth.Components
{
    public class FileSystemCertificateService : ICertificateService
    {
        private readonly string CertificatePath;
        private readonly string CertificatePassword;

        public FileSystemCertificateService(
            IOptions<Options> options
        ) {
            CertificatePath = options.Value.CertificatePath;
            CertificatePassword = options.Value.CertificatePassword;
        }

        public FileSystemCertificateService(
            string certificatePath,
            string certificatePassword = null
        ) {
            CertificatePath = certificatePath;
            CertificatePassword = certificatePassword;
        }

        public X509Certificate2 GetCertificate()
        {
            var certContents = File.ReadAllBytes(CertificatePath);
            var certificate = new X509Certificate2(
                certContents,
                CertificatePassword);
            return certificate;
        }
    }
}