using BusinessLogicLayer.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer;
using ServiceLayer.CloudStorageProviders;
using Microsoft.Graph;

namespace BusinessLogicLayer.Services
{
    public class FileStorageService
    {
        // Fields
        private OneDriveAPI oneDriveAPI;

        // Private methods
        private async void RequestAndGenerateChildrenEntitiesRecursively(BusinessEntities.Folder parentFolder, DriveItem expandedFolderDriveItem)
        {
            // Generate entities for its children
            foreach (var childDriveItem in expandedFolderDriveItem.Children.CurrentPage)
            {
                // Folder
                if (childDriveItem.Folder != null)
                {
                    var newChildFolder = new BusinessEntities.Folder(childDriveItem.Id, childDriveItem.Name, childDriveItem.Size, parentFolder);
                    parentFolder.Folders.Add(newChildFolder);

                    // Generate its children when it is a Folder
                    if (expandedFolderDriveItem.Children != null && expandedFolderDriveItem.Children.CurrentPage != null)
                    {
                        var expandedChildDriveItem = await oneDriveAPI.GetDriveItemAsync(childDriveItem.Id);
                        RequestAndGenerateChildrenEntitiesRecursively(newChildFolder, expandedChildDriveItem);
                    }
                }
                // File
                else if (childDriveItem.File != null)
                {
                    var newFile = new BusinessEntities.File(childDriveItem.Id, childDriveItem.Name, childDriveItem.Size, parentFolder);
                    parentFolder.Files.Add(newFile);
                }
            }
        }

        // Constructor
        public FileStorageService()
        {
            this.oneDriveAPI = new OneDriveAPI();
        }

        // Public methods
        public async Task<BusinessEntities.Folder> GetRootFolderWithDescendants()
        {
            //
            var rootDriveItem = await oneDriveAPI.GetRootFolderAsync();
            var rootFolder = new BusinessEntities.Folder(rootDriveItem.Id, "Root", rootDriveItem.Size, null);
            this.RequestAndGenerateChildrenEntitiesRecursively(rootFolder, rootDriveItem);


            return rootFolder;
        }
        public async Task<BusinessEntities.File> UploadFile(BusinessEntities.Folder targetFolder, string fileNameWithExtension, System.IO.Stream fileStream)
        {
            // Get the targetFolder driveItem and the destination path (strip /drive/root: (12 characters) from the parent path string)
            var targetFolderDriveItem = await this.oneDriveAPI.GetDriveItemAsync(targetFolder.ID);
            string destinationFolderPath = (targetFolderDriveItem.Name == "root" && targetFolderDriveItem.ParentReference.Name == null)
                ? ""
                : targetFolderDriveItem.ParentReference.Path.Remove(0, 12) + "/" + Uri.EscapeUriString(targetFolderDriveItem.Name);
            var uploadPath = destinationFolderPath + "/" + fileNameWithExtension;


            // Attempt to upload the file 
            BusinessEntities.File uploadedFile = null;
            try
            {
                var uploadedItem = await this.oneDriveAPI.UploadDriveItemAsync(uploadPath, fileStream);
                uploadedFile = new BusinessEntities.File(uploadedItem.Id, uploadedItem.Name, uploadedItem.Size, null);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return uploadedFile;
        }
        public async Task<System.IO.Stream> DownloadFile(BusinessEntities.File targetFile)
        {
            System.IO.Stream fileStream = null;
            try
            {
                fileStream = await this.oneDriveAPI.GetDriveItemContentAsync(targetFile.ID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return fileStream;
        }
    }

}
