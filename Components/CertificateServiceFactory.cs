
using System;
using Microsoft.Extensions.Options;
using Microsoft.Pfe.Samples.AzureFunctions.Cds.Auth.Services;
using Newtonsoft.Json;

namespace Microsoft.Pfe.Samples.AzureFunctions.Cds.Auth.Components
{
    public class CertificateServiceFactory : ICertificateServiceFactory
    {
        private readonly IOptions<Options> Options;

        public CertificateServiceFactory(
            IOptions<Options> options
        ) {
            Options = options;
        }

        public ICertificateService CreateCertificateService()
        {
            if(Options.Value.CertificatePath != null) {
                return new FileSystemCertificateService(Options);
            } else if(Options.Value.CertificateThumbprint != null) {
                if(Options.Value.Platform == PlatformID.Unix) {
                    return new LinuxFunctionAppCertificateService(Options);
                }
            }
            throw new NotSupportedException(
                $"Given configuration is not supported: {JsonConvert.SerializeObject(Options)}");
        }
    }
}