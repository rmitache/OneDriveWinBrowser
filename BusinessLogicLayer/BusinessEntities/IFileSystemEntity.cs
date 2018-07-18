using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.BusinessEntities
{
    public interface IFileSystemEntity
    {
        string Name { get; set; }
        long? Size { get; set; }
        string TypeName { get; }
        string FormattedName { get; }
        List<IFileSystemEntity>  Children { get; }
    }
}
