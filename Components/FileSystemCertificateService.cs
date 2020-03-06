using System.IO;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Pfe.Samples.AzureFunctions.Cds.Auth.Services;

namespace Microsoft.Pfe.Samples.AzureFunctions.Cds.Auth.Components
{
    public class FileSystemCertificateService : ICertificateService
    {
        private readonly string Path;
        private readonly string Password;

        public FileSystemCertificateService(string path, string password = null)
        {
            Path = path;
            Password = password;
        }

        public X509Certificate2 GetCertificate()
        {
            var certContents = File.ReadAllBytes(Path);
            var certificate = new X509Certificate2(
                certContents,
                Password);
            return certificate;
        }
    }
}