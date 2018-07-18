using ByteSizeLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.BusinessEntities
{
    public class Folder : IFileSystemEntity
    {
        public string Name { get; set; }
        public long? Size { get; set; }
        public List<File> Files { get; set; }
        public List<Folder> Folders { get; set; }
        public string TypeName
        {
            get
            {
                return "Folder";
            }
        }

        public string FormattedName
        {
            get
            {
                string formattedString = this.Name;
                if (this.Size != null)
                {
                    formattedString += " (" + ByteSize.FromBytes((double)this.Size) + ")";
                }
                return formattedString;
            }
        }

        public Folder(string name, long? size = null)
        {
            this.Name = name;
            this.Size = size;

            this.Files = new List<File>();
            this.Folders = new List<Folder>();
        }
    }
}
