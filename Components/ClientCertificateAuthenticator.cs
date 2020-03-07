
using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Microsoft.Pfe.Samples.AzureFunctions.Cds.Auth.Services;

namespace Microsoft.Pfe.Samples.AzureFunctions.Cds.Auth.Components
{
    public class ClientCertificateAuthenticator : ITokenService
    {
        private readonly Uri Authority;
        private readonly string ClientId;
        private readonly Uri CdsEnvironmentUri;

        private readonly X509Certificate2 Certificate;

        public ClientCertificateAuthenticator(
            IOptions<Options> options,
            X509Certificate2 certificate
        ) {
            Authority = options.Value.Authority;
            ClientId = options.Value.ClientId;
            CdsEnvironmentUri = options.Value.CdsEnvironmentUri;
            Certificate = certificate;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            IConfidentialClientApplication app = BuildApp();
            string[] scopes = CreateScopes();
            AuthenticationResult result = await app.AcquireTokenForClient(scopes).ExecuteAsync();
            return result.AccessToken;
        }
        
        private IConfidentialClientApplication BuildApp() {
            return ConfidentialClientApplicationBuilder
                .Create(ClientId)
                .WithCertificate(Certificate)
                .WithAuthority(Authority)
                .Build();
        }

        private string[] CreateScopes() {
            return new [] {
                new Uri(
                    CdsEnvironmentUri,
                    "/.default"
                ).AbsoluteUri };
        }
    }
}