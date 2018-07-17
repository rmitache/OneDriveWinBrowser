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
    }
}
