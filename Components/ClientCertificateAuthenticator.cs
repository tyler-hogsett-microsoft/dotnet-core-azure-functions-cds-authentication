
using System;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Microsoft.Pfe.Samples.AzureFunctions.Cds.Auth.Models;
using Microsoft.Pfe.Samples.AzureFunctions.Cds.Auth.Services;

namespace Microsoft.Pfe.Samples.AzureFunctions.Cds.Auth.Components
{
    public class ClientCertificateAuthenticator : ITokenService
    {
        private readonly CertificateAuthenticationConfiguration Config;

        public ClientCertificateAuthenticator(CertificateAuthenticationConfiguration config) {
            Config = config;
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
                .Create(Config.ClientId)
                .WithCertificate(Config.ClientCertificate)
                .WithAuthority(Config.Authority)
                .Build();
        }

        private string[] CreateScopes() {
            return new [] {
                new Uri(
                    Config.CdsEnvironmentUri,
                    "/.default"
                ).AbsoluteUri };
        }
    }
}