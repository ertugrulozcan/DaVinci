using Ertis.DaVinci.HtmlModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.DaVinci.Models
{
    public class Project
    {
        #region Fields

        private int id;
        private string name;
        private List<FileSystemEntity> files;
        private ReadOnlyCollection<FileSystemEntity> readonlyFilesCollection;
        private WebSite site;

        #endregion

        #region Properties

        public int Id
        {
            get
            {
                return id;
            }

            private set
            {
                this.id = value;
                this.RaisePropertyChanged("Id");
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            private set
            {
                this.name = value;
                this.RaisePropertyChanged("Name");
            }
        }
        
        [JsonIgnore]
        public ReadOnlyCollection<FileSystemEntity> Files
        {
            get
            {
                return readonlyFilesCollection;
            }

            private set
            {
                this.readonlyFilesCollection = value;
                this.RaisePropertyChanged("Files");
            }
        }

        public WebSite Site
        {
            get
            {
                return site;
            }

            set
            {
                this.site = value;
                this.RaisePropertyChanged("Site");
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        private Project()
        {
            
        }

        public Project(int id, string name)
        {
            this.files = new List<FileSystemEntity>();
            this.Files = new ReadOnlyCollection<FileSystemEntity>(this.files);

            this.Id = id;
            this.Name = name;
        }

        public static Project Create(int id, string name, string folderPath)
        {
            Project project = new Project(id, name);
            project.Site = new WebSite()
            {
                Name = name,
                Title = name,
                PageList = new ObservableCollection<Page>(),
                SiteSettings = new SiteSettings()
            };

            project.SetFileSystemTree(folderPath);

            return project;
        }
        
        #endregion

        #region Methods

        private void SetFileSystemTree(string folderPath)
        {
            if (string.IsNullOrEmpty(folderPath) || this.files == null)
                return;

            this.files.AddRange(this.GetFileSystemTree(folderPath));
        }

        private List<FileSystemEntity> GetFileSystemTree(string rootPath)
        {
            if (string.IsNullOrEmpty(rootPath))
                return new List<FileSystemEntity>();

            var fileInfos = new List<FileSystemEntity>();
            var filesAndDirectories = Directory.GetFileSystemEntries(rootPath);

            foreach (var fileName in filesAndDirectories)
            {
                if (File.Exists(fileName))
                    fileInfos.Add(new FileSystemEntity(fileName));
                else if (Directory.Exists(fileName))
                {
                    var directoryInfo = new FileSystemEntity(fileName);
                    directoryInfo.Children.AddRange(this.GetFileSystemTree(fileName));
                    fileInfos.Add(directoryInfo);
                }
            }

            return fileInfos;
        }

        #endregion

        #region RaisePropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
