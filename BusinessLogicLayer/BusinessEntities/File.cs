using ByteSizeLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.BusinessEntities
{
    public class File: IFileSystemEntity
    {
        public string Name { get; set; }
        public long? Size { get; set; }

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

        public File(string name, long? size)
        {
            this.Name = name;
            this.Size = size;
        }
    }
}
