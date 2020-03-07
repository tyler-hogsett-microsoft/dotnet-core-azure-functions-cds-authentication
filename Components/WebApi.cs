
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Pfe.Samples.AzureFunctions.Cds.Auth.Services;

namespace Microsoft.Pfe.Samples.AzureFunctions.Cds.Auth.Components
{
    public class WebApiService : IWebApiService
    {
        private readonly HttpClient Client;

        public WebApiService(HttpClient client) {
            Client = client;
        }

        public Task<HttpResponseMessage> GetAsync(string path) {
            return Client.GetAsync(
                "/api/data/v9.1/contacts?$select=fullname"
            );
        }
    }
}