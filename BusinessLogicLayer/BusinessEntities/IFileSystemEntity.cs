using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.BusinessEntities
{
    public interface IFileSystemEntity
    {
        string ID { get; }
        string Name { get; set; }
        long? Size { get; set; }
        string TypeName { get; }
        IFileSystemEntityType EntityType { get; }
        string FormattedName { get; }
        List<IFileSystemEntity> Children { get; }
        Folder ParentFolder { get; set; }
    }
}
