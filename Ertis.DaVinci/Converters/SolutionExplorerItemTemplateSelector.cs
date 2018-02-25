using Ertis.DaVinci.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Ertis.DaVinci.Converters
{
    public class SolutionExplorerItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate FileItemTemplate { get; set; }
        public DataTemplate FolderItemTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is FileSystemEntity && (item as FileSystemEntity).FileInfo != null)
            {
                var fileInfo = (item as FileSystemEntity).FileInfo;
                if (fileInfo is FileInfo)
                    return this.FileItemTemplate;
                if (fileInfo is DirectoryInfo)
                    return this.FolderItemTemplate;
            }

            return null;
        }
    }
}
