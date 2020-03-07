namespace Microsoft.Pfe.Samples.AzureFunctions.Cds.Auth.Services
{
    public interface ICertificateServiceFactory
    {
        ICertificateService CreateCertificateService();
    }
}
