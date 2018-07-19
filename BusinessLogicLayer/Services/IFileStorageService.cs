using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public interface IFileStorageService
    {
        Task<BusinessEntities.Folder> GetRootFolderWithDescendants();
        Task<BusinessEntities.File> UploadFile(BusinessEntities.Folder targetFolder, string fileNameWithExtension, System.IO.Stream fileStream);
        Task<System.IO.Stream> DownloadFile(BusinessEntities.File targetFile);
    }
}
