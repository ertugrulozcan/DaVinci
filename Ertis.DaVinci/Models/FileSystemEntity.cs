using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.DaVinci.Models
{
    public class FileSystemEntity
    {
        public FileSystemInfo FileInfo { get; private set; }
        public List<FileSystemEntity> Children { get; private set; }

        public FileSystemEntity(string path)
        {
            this.Children = new List<FileSystemEntity>();

            if (File.Exists(path))
                FileInfo = new FileInfo(path);
            else if (Directory.Exists(path))
                FileInfo = new DirectoryInfo(path);
        }
    }
}
