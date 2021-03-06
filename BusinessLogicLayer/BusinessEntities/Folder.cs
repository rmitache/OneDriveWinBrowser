﻿using ByteSizeLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.BusinessEntities
{
    public class Folder : IFileSystemEntity
    {
        // Properties
        public string ID { get; }
        public string Name { get; set; }
        public long? Size { get; set; }
        public List<File> Files { get; set; }
        public List<Folder> Folders { get; set; }

        // IFileSystemEntity implementation
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
        public List<IFileSystemEntity> Children
        {
            get
            {
                var list = new List<IFileSystemEntity>();
                list.AddRange(this.Folders);
                list.AddRange(this.Files);
                return list;
            }
        }
        public IFileSystemEntityType EntityType
        {
            get
            {
                return IFileSystemEntityType.Folder;
            }
        }
        public Folder ParentFolder { get; set; }

        // Constructor
        public Folder(string id, string name, long? size, Folder parentFolder)
        {
            this.ID = id;
            this.Name = name;
            this.Size = size;

            this.Files = new List<File>();
            this.Folders = new List<Folder>();

            this.ParentFolder = parentFolder;
        }

        
    }
}
