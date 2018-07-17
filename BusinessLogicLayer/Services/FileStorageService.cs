using BusinessLogicLayer.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer;
using ServiceLayer.CloudStorageProviders;

namespace BusinessLogicLayer.Services
{
    public class FileStorageService
    {
        public async Task<List<IFileSystemEntity>> GetFileStorageRootFolder()
        {
            // Variables
            var list = new List<IFileSystemEntity>();

            // Parse the elements in the fileStorage root and convert them to IFileSystemEntities
            var rootFolder = await OneDriveAPI.GetAllInFileStructure();
            if (rootFolder != null)
            {
                if (rootFolder.Folder != null && rootFolder.Children != null && rootFolder.Children.CurrentPage != null)
                {
                    foreach (var obj in rootFolder.Children.CurrentPage)
                    {
                        IFileSystemEntity newNode;
                        if (obj.Folder != null)
                        {
                            newNode = new Folder
                            {
                                Name = obj.Name,
                                Size = obj.Size
                           
                            };
                        }
                        else
                        {
                            newNode = new File
                            {
                                Name = obj.Name,
                                Size = obj.Size
                            };
                        }
                        

                        list.Add(newNode);
                    }
                }
            }
            return list;
        }
    }

}
