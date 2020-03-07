using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Microsoft.Pfe.Samples.AzureFunctions.Cds.Auth.Utilities.OsVersion
{
    public static class Function
    {
        [FunctionName("os-version")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            return (IActionResult)new OkObjectResult(
                $"Platform: {Environment.OSVersion.Platform}\r\n" +
                $"ServicePack: {Environment.OSVersion.ServicePack}\r\n" +
                $"Version: {Environment.OSVersion.Version}\r\n" +
                $"VersionString: {Environment.OSVersion.VersionString}");
        }
    }
}