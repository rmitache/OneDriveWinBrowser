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
        private void RequestAndGenerateChildrenEntitiesRecursively(BusinessEntities.Folder parentFolder, DriveItem expandedFolderDriveItem)
        {

            // Generate entities for its children
            foreach (var childDriveItem in expandedFolderDriveItem.Children.CurrentPage)
            {
                // Folder
                if (childDriveItem.Folder != null)
                {
                    var newChildFolder = new BusinessEntities.Folder(childDriveItem.Name, childDriveItem.Size);
                    parentFolder.Folders.Add(newChildFolder);

                    // Generate its children when it is a Folder
                    if (expandedFolderDriveItem.Children != null && expandedFolderDriveItem.Children.CurrentPage != null)
                    {
                        var expandedChildDriveItem = oneDriveAPI.GetExpandedDriveItem(childDriveItem.Id);
                        RequestAndGenerateChildrenEntitiesRecursively(newChildFolder, expandedChildDriveItem);
                    }
                }
                // File
                else if (childDriveItem.File != null)
                {
                    var newFile = new BusinessEntities.File(childDriveItem.Name, childDriveItem.Size);
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
            var rootFolder = new BusinessEntities.Folder("Root", 0);
            var rootDriveItem = await oneDriveAPI.GetRootFolderAsync();
            this.RequestAndGenerateChildrenEntitiesRecursively(rootFolder, rootDriveItem);


            return rootFolder;
        }
    }

}
