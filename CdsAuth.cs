using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Options;
using Microsoft.Pfe.Samples.AzureFunctions.Cds.Auth.Services;

namespace Microsoft.Pfe.Samples.AzureFunctions.Cds.Auth
{
    public class CdsAuth
    {
        private readonly IWebApiService WebApiService;

        public CdsAuth(IWebApiService webApiService) {
            WebApiService = webApiService;
        }

        [FunctionName("cds-auth")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Querying API for contacts...");
            HttpResponseMessage response = await WebApiService.GetAsync(
                "/api/data/v9.1/contacts?$select=fullname"
            );
            var content = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(content);
            log.LogInformation($"Retrieved {((JArray)result.value).Count} contacts.");

            return (ActionResult)new OkObjectResult(JsonConvert.SerializeObject(result, Formatting.Indented));
        }
    }
}
