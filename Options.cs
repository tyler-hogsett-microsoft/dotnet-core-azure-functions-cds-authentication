
using System;

namespace Microsoft.Pfe.Samples.AzureFunctions.Cds.Auth
{
    public class Options
    {
        public Uri Authority {get; set;}
        public string ClientId {get; set;}
        public string CertificatePath {get; set;}
        public string CertificateThumbprint {get; set;}
        public string CertificatePassword {get; set;}
        public string ClientSecret {get; set;}
        public Uri CdsEnvironmentUri {get; set;}
        public PlatformID Platform { get; set; }
    }
}