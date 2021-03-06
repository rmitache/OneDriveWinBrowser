﻿using Microsoft.Graph;
using Microsoft.Identity.Client;
using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ServiceLayer.CloudStorageProviders
{
    public class OneDriveAPI: IMicrosoftGraphAPIProvider
    {
        // Fields
        private const string clientId = "1826dd27-ce57-42eb-a4df-441e00a4ff8f";
        private readonly string[] scopes = { "Files.ReadWrite.All" };
        private readonly PublicClientApplication IdentityClientApp = new PublicClientApplication(clientId);
        public IUser user = null;
        private GraphServiceClient graphClient = null;

        // Private methods
        private async Task InitGraphServiceClientAsync()
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
        private async Task<string> GetTokenForUserAsync()
        {
            AuthenticationResult authResult;
            try
            {

                authResult = await IdentityClientApp.AcquireTokenSilentAsync(scopes, this.user);
            }

            catch (Exception ex)
            {
                authResult = await IdentityClientApp.AcquireTokenAsync(scopes);
                this.user = authResult.User;
            }


            return authResult.AccessToken;
        }
        


        // Public methods
        public async Task<DriveItem> UploadDriveItemAsync(string uploadPath, System.IO.Stream fileStream)
        {
            var uploadedItem = await graphClient.Drive.Root.ItemWithPath(uploadPath).Content.Request().PutAsync<DriveItem>(fileStream);
            return uploadedItem;
        }
        public async Task<DriveItem> GetDriveItemAsync(string id, bool expandChildren = true)
        {
            Task<DriveItem> driveItemTask;
            if (expandChildren)
            {
                driveItemTask =  this.graphClient.Drive.Items[id].Request().Expand("children").GetAsync();
            }
            else
            {
                driveItemTask =   this.graphClient.Drive.Items[id].Request().GetAsync();
            }


            driveItemTask.Wait();
            return driveItemTask.Result;
        }
        public async Task<System.IO.Stream> GetDriveItemContentAsync(string id)
        {
            var stream = await this.graphClient.Drive.Items[id].Content.Request().GetAsync();
            return stream;
        }
        public async Task<DriveItem> GetRootFolderAsync()
        {
            await InitGraphServiceClientAsync();

            DriveItem root = await graphClient.Drive.Root.Request().Expand("children").GetAsync();

            return root;
        }
    }
}
