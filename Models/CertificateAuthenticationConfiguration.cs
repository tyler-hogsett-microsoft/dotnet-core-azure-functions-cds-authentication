
using System;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.Pfe.Samples.AzureFunctions.Cds.Auth.Models
{
    public class CertificateAuthenticationConfiguration {
        public Uri Authority {get; set;}
        public string ClientId {get; set;}
        public X509Certificate2 ClientCertificate {get; set;}
        public Uri CdsEnvironmentUri {get; set;}
    }
}