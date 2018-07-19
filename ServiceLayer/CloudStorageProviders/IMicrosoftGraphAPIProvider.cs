using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.CloudStorageProviders
{
    public interface IMicrosoftGraphAPIProvider
    {
        Task<DriveItem> UploadDriveItemAsync(string uploadPath, System.IO.Stream fileStream);
        Task<DriveItem> GetDriveItemAsync(string id, bool expandChildren = true);
        Task<System.IO.Stream> GetDriveItemContentAsync(string id);
        Task<DriveItem> GetRootFolderAsync();

    }
}
