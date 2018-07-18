using ByteSizeLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.BusinessEntities
{
    public class File : IFileSystemEntity
    {
        // Properties
        public string Name { get; set; }
        public long? Size { get; set; }

        // IFileSystemEntity implementation
        public string TypeName
        {
            get
            {
                return "File";
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
        public List<IFileSystemEntity> Children
        {
            get
            {
                var list = new List<IFileSystemEntity>();
                return list;
            }
        }
        public IFileSystemEntityType EntityType { get
            {
                return IFileSystemEntityType.File;
            }
        }

        // Constructor
        public File(string name, long? size)
        {
            this.Name = name;
            this.Size = size;
        }
    }
}
