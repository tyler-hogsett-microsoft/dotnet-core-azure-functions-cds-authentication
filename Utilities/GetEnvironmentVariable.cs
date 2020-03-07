using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Microsoft.Pfe.Samples.AzureFunctions.Cds.Auth.Utilities.GetEnvironmentVariable
{
    public static class Function
    {
        [FunctionName("get-environment-variable")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            if(!req.Query.ContainsKey("variable")) {
                return new BadRequestObjectResult("Please provide a 'variable' name");
            }

            var variable = req.Query["variable"][0];
            var value = Environment.GetEnvironmentVariable(variable);
            return new OkObjectResult(
                $"{variable} = {value}"
            );
        }
    }
}