using System.Threading.Tasks;

namespace Microsoft.Pfe.Samples.AzureFunctions.Cds.Auth.Services
{
    internal interface ITokenService {
        Task<string> GetAccessTokenAsync();
    }
}
