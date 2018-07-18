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
        private List<IFileSystemEntity> GenerateEntitiesFromDriveItemFolder(DriveItem item)
        {
            var list = new List<IFileSystemEntity>();


            if (item.Folder != null && item.Children != null && item.Children.CurrentPage != null)
            {
                foreach (var obj in item.Children.CurrentPage)
                {
                    IFileSystemEntity newEntity;
                    if (obj.Folder != null)
                    {
                        newEntity = new BusinessEntities.Folder(obj.Name, obj.Size);
                    }
                    else
                    {
                        newEntity = new BusinessEntities.File(obj.Name, obj.Size);
                    }

                    list.Add(newEntity);
                }
            }

            return list;
        }

        // Constructor
        public FileStorageService()
        {
            this.oneDriveAPI = new OneDriveAPI();
        }

        // Public methods
        public async Task<List<IFileSystemEntity>> GetFileStorageRootFolder()
        {
            // Variables
            var list = new List<IFileSystemEntity>();

            // Parse the elements in the fileStorage root and convert them to IFileSystemEntities
            var rootFolder = await oneDriveAPI.GetRootFolderAsync();
            if (rootFolder != null)
            {
                list = GenerateEntitiesFromDriveItemFolder(rootFolder);
            }
            return list;
        }
    }

}
