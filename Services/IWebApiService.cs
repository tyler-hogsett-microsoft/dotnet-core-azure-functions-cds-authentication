using System.Net.Http;
using System.Threading.Tasks;

namespace Microsoft.Pfe.Samples.AzureFunctions.Cds.Auth.Services
{
    public interface IWebApiService {
        Task<HttpResponseMessage> GetAsync(string path);
    }
}