using Microsoft.Graph;
using Microsoft.Identity.Client;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ServiceLayer.CloudStorageProviders
{
    public class OneDriveAPI
    {
        // Fields
        private const string clientId = "1826dd27-ce57-42eb-a4df-441e00a4ff8f";
        private readonly string[] scopes = { "Files.ReadWrite.All" };
        private readonly PublicClientApplication IdentityClientApp = new PublicClientApplication(clientId);

        private GraphServiceClient graphClient = null;

        // Private methods
        private  async Task InitGraphServiceClientAsync()
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
        private  async Task<string> GetTokenForUserAsync()
        {
            AuthenticationResult authResult;

            authResult = await IdentityClientApp.AcquireTokenAsync(scopes);
            var accessToken = authResult.AccessToken;


            return accessToken;
        }

        // Public methods
        public  async Task<DriveItem> GetAllInFileStructure()
        {
            await InitGraphServiceClientAsync();

            // OBS: Needs to be recursive
            DriveItem driveItem = await graphClient.Drive.Root.Request().GetAsync();
            //while (driveItem..Request().Expand("children").GetAsync())
            //{

            //}

            var rootDriveItem = await graphClient.Drive.Root.Request().Expand("children").GetAsync();
            return rootDriveItem;
        }
    }
}
