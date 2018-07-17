using Microsoft.Graph;
using Microsoft.Identity.Client;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ServiceLayer.CloudStorageProviders
{
    public class OneDriveAPI
    {
        // Fields
        private static string clientId = "1826dd27-ce57-42eb-a4df-441e00a4ff8f";
        private static PublicClientApplication IdentityClientApp = new PublicClientApplication(clientId);
        private static string[] scopes = { "Files.ReadWrite.All" };
        private static GraphServiceClient graphClient = null;

        // Private methods
        private static async Task InitGraphServiceClientAsync()
        {
            if (graphClient == null)
            {
                graphClient = new GraphServiceClient(
                            "https://graph.microsoft.com/v1.0",
                            new DelegateAuthenticationProvider(
                                async (requestMessage) =>
                                {
                                    var token = await GetTokenForUserAsync();
                                    requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);

                                }));
            }
        }
        private static async Task<string> GetTokenForUserAsync()
        {
            AuthenticationResult authResult;

            authResult = await IdentityClientApp.AcquireTokenAsync(scopes);
            var accessToken = authResult.AccessToken;


            return accessToken;
        }

        // Public methods
        public static async Task<DriveItem> GetAllInFileStructure()
        {
            await InitGraphServiceClientAsync();
            var rootDriveItem = await graphClient.Drive.Root.Request().Expand("children").GetAsync();
            return rootDriveItem;
        }
    }
}
