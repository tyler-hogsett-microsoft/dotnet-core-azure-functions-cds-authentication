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

namespace Microsoft.Pfe.Samples.AzureFunctions.Cds.Auth
{
    public static class Function
    {
        [FunctionName("cds_auth")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string tenant = Environment.GetEnvironmentVariable("Tenant");
            string instance = Environment.GetEnvironmentVariable("Instance");
            var authority = new Uri(string.Format(instance, tenant));
            string clientId = Environment.GetEnvironmentVariable("ClientId");
            string clientSecret = Environment.GetEnvironmentVariable("ClientSecret");
            var cdsEnvironmentUrl = new Uri(Environment.GetEnvironmentVariable("CdsEnvironmentUrl"));
            IConfidentialClientApplication app = ConfidentialClientApplicationBuilder.Create(clientId)
                .WithClientSecret(clientSecret)
                .WithAuthority(authority)
                .Build();
            var scopes = new [] { new Uri(cdsEnvironmentUrl, "/.default").AbsoluteUri };
            string token = app.AcquireTokenForClient(scopes).ExecuteAsync().Result.AccessToken;

            log.LogInformation($"Token: {token}");

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
            
            HttpResponseMessage response = client.GetAsync(
                new Uri(cdsEnvironmentUrl, "/api/data/v9.1/").AbsoluteUri
            ).Result;
            log.LogInformation(response.Content.ReadAsStringAsync().Result);

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            return name != null
                ? (ActionResult)new OkObjectResult($"Hello, {name}")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }
}
