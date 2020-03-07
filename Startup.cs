
using System;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;
using Microsoft.Pfe.Samples.AzureFunctions.Cds.Auth.Components;
using Microsoft.Pfe.Samples.AzureFunctions.Cds.Auth.Models;
using Microsoft.Pfe.Samples.AzureFunctions.Cds.Auth.Services;

[assembly: FunctionsStartup(typeof(Microsoft.Pfe.Samples.AzureFunctions.Cds.Auth.Startup))]

namespace Microsoft.Pfe.Samples.AzureFunctions.Cds.Auth
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services
                .AddOptions<Options>()
                .Configure<IConfiguration>(
                    (settings, configuration) => {
                        configuration
                            .Bind(settings);
                        settings.Platform = Environment.OSVersion.Platform;

                        var functionCertThumbprint =
                            Environment.GetEnvironmentVariable("WEBSITE_LOAD_CERTIFICATES");
                        if(functionCertThumbprint != null) {
                            settings.CertificateThumbprint = functionCertThumbprint;
                        }
                    }
                );
            builder.Services
                .AddSingleton<ICertificateServiceFactory, CertificateServiceFactory>()
                .AddSingleton<ICertificateService>(
                    serviceProvider => {
                        var factory = serviceProvider.GetService<ICertificateServiceFactory>();
                        return factory.CreateCertificateService();
                    }
                )
                .AddSingleton<X509Certificate2>(
                    serviceProvider => {
                        var certService = serviceProvider.GetService<ICertificateService>();
                        return certService.GetCertificate();
                    }
                )
                .AddSingleton<ITokenService, ClientCertificateAuthenticator>()
                .AddHttpClient<IWebApiService, WebApiService>()
                .ConfigureHttpClient(
                    (serviceProvider, client) => {
                        var tokenService = serviceProvider.GetService<ITokenService>();
                        var token = tokenService.GetAccessTokenAsync().Result;    
                        client.DefaultRequestHeaders.Authorization =
                            new AuthenticationHeaderValue("Bearer", token);
                        
                        client.DefaultRequestHeaders.Add("Accept", "application/json");

                        var options = serviceProvider.GetService<IOptions<Options>>();
                        client.BaseAddress = options.Value.CdsEnvironmentUri;
                    });
        }
    }
}