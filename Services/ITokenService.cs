using System.Threading.Tasks;

namespace Microsoft.Pfe.Samples.AzureFunctions.Cds.Auth.Services
{
    public interface ITokenService {
        Task<string> GetAccessTokenAsync();
    }
}
