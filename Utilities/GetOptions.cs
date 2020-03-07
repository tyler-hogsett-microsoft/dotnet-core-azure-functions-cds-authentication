using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Microsoft.Pfe.Samples.AzureFunctions.Cds.Auth.Utilities
{
    public class GetOptionsFunction
    {
        private readonly Options Options;

        public GetOptionsFunction(IOptions<Options> options)
        {
            Options = options.Value;
        }

        [FunctionName("get-options")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest request,
            ILogger log
        ) {
            return new OkObjectResult(
                JsonConvert.SerializeObject(
                    Options
                )
            );
        }
    }
}