using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Identity.Client;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Primitives;
using Microsoft.Pfe.Samples.AzureFunctions.Cds.Auth.Models;
using Microsoft.Pfe.Samples.AzureFunctions.Cds.Auth.Components;

namespace Microsoft.Pfe.Samples.AzureFunctions.Cds.Auth
{
    public static class Function
    {
        [FunctionName("cds-auth")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            string tenant = Environment.GetEnvironmentVariable("Tenant");
            string instance = Environment.GetEnvironmentVariable("Instance");
            var authority = new Uri(string.Format(instance, tenant));
            string clientId = Environment.GetEnvironmentVariable("ClientId");
            var cdsEnvironmentUrl = new Uri(Environment.GetEnvironmentVariable("CdsEnvironmentUrl"));
            string certificatePath = Environment.GetEnvironmentVariable("CertificatePath");
            X509Certificate2 certificate = null;
            if(certificatePath == null) {
                string certificateThumbprint = Environment.GetEnvironmentVariable("WEBSITE_LOAD_CERTIFICATES");

                const string dir = "/var/ssl/private";

                certificatePath = $"{dir}/{certificateThumbprint}.p12";
            }
            var certContents = File.ReadAllBytes(certificatePath);
            certificate = new X509Certificate2(
                certContents,
                Environment.GetEnvironmentVariable("CertificatePassword"));
            
            var config = new CertificateAuthenticationConfiguration {
                Authority = authority,
                CdsEnvironmentUri = cdsEnvironmentUrl,
                ClientCertificate = certificate,
                ClientId = clientId
            };

            var authenticator = new ClientCertificateAuthenticator(config);
            
            log.LogInformation("Retrieving access token...");
            string token = await authenticator.GetAccessTokenAsync();
            log.LogInformation("Successfully retrieved access token.");

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            log.LogInformation("Querying API for contacts...");
            HttpResponseMessage response = await client.GetAsync(
                new Uri(
                    cdsEnvironmentUrl,
                    "/api/data/v9.1/contacts?$select=fullname"
                ).AbsoluteUri
            );
            dynamic result = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
            log.LogInformation($"Retrieved {((JArray)result.value).Count} contacts.");

            return (ActionResult)new OkObjectResult(JsonConvert.SerializeObject(result, Formatting.Indented));
        }
    }
}
