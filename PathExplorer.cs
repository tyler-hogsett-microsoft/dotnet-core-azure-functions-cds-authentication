
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.Pfe.Samples.AzureFunctions.Cds.Auth.Utilities.PathExplorer
{
    public static class Function
    {
        [FunctionName("path-explorer")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            StringValues pathValues;
            if (req.Query.TryGetValue("path", out pathValues) && pathValues.Count > 0)
            {
                string path = pathValues[0];
                dynamic theValue = new JObject();
                bool pathExists = Directory.Exists(path);
                theValue.PathExits = pathExists;
                if (pathExists)
                {
                    object[] directories = Directory.GetDirectories(path);
                    theValue.ChildDirectories = new JArray(directories);
                    object[] files = Directory.GetFiles(path);
                    theValue.ChildFiles = new JArray(files);
                }
                return (IActionResult)new OkObjectResult(
                    JsonConvert.SerializeObject(theValue, Formatting.Indented));
            }
            else
            {
                return new BadRequestObjectResult("Please enter a path");
            }
        }
    }
}