using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.BusinessEntities
{
    public class Folder : IFileSystemEntity
    {
        public string Name { get; set; }
        public long? Size { get; set; }
        public List<File> Files { get; set; }
        public List<Folder> Folders { get; set; }
    }
}
