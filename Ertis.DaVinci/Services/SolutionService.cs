using Ertis.DaVinci.Events;
using Ertis.DaVinci.HtmlModels;
using Ertis.DaVinci.Models;
using Ertis.DaVinci.Serialization;
using Ertis.DaVinci.Services.Interfaces;
using Ertis.Shared.Components;
using Ertis.Shared.Models;
using Ertis.Shared.Services.Contracts;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Practices.Prism.Events;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.DaVinci.Services
{
    public class SolutionService : ISolutionService, INotifyPropertyChanged
    {
        #region Services

        private readonly IEventAggregator eventAggregator;
        private readonly IWindowNavigationService windowNavigationService;

        #endregion

        #region Fields

        private Solution currentSolution;

        #endregion

        #region Properties

        public Solution CurrentSolution
        {
            get
            {
                return currentSolution;
            }

            private set
            {
                currentSolution = value;
                this.RaisePropertyChanged("CurrentSolution");
            }
        }

        public string SolutionPath { get; private set; }

        public string TempFolderPath { get; private set; }

        #endregion

        #region Events

        public event EventHandler<Solution> SolutionLoaded;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="windowNavigationService"></param>
        public SolutionService(IEventAggregator eventAggregator, IWindowNavigationService windowNavigationService)
        {
            this.eventAggregator = eventAggregator;
            this.windowNavigationService = windowNavigationService;

            this.TempFolderPath = System.IO.Path.GetTempPath() + "DaVinciTemporary" + "\\";
        }

        #endregion

        #region Methods

        public void CreateSolution(string projectName, string folderPath)
        {
            try
            {
                this.SolutionPath = string.Format(@"{0}\{1}", folderPath, projectName);
                string solutionFilePath = string.Format(@"{0}\{1}\{1}.ei", folderPath, projectName);

                if (!Directory.Exists(this.SolutionPath))
                    Directory.CreateDirectory(this.SolutionPath);

                Solution solution = new Solution(1001, projectName);
                // C:\Users\ertugrulo\Desktop\DaVinci\Culcuoglu
                solution.SolutionPath = this.SolutionPath;
                Project defaultProject = Project.Create(101, projectName, this.SolutionPath);
                solution.ProjectList.Add(defaultProject);

                this.SaveSolution(solution);

                this.LoadSolution(solutionFilePath);
            }
            catch (Exception ex)
            {
                this.SolutionPath = null;
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
        
        public void LoadSolution(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    string jsonText = File.ReadAllText(path);
                    
                    JsonConverter[] converters = { new SectionSerializer() };
                    Solution solution = JsonConvert.DeserializeObject<Solution>(jsonText, new JsonSerializerSettings()
                    {
                        Converters = converters,
                        TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto,
                        NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
                    });
                    
                    this.CurrentSolution = solution;
                    // C:\Users\ertugrulo\Desktop\DaVinci\Culcuoglu
                    this.SolutionPath = Path.GetDirectoryName(path); ;
                    this.CurrentSolution.SolutionPath = this.SolutionPath;
                    this.LoadSolution(this.CurrentSolution);
                }
                else if (Directory.Exists(path))
                {
                    var files = Directory.GetFiles(path);
                    if (files.Any(x => x.EndsWith(".ei")))
                    {
                        var solutionFileName = files.First(x => x.EndsWith(".ei"));
                        this.LoadSolution(path + "\\" + solutionFileName);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void LoadSolution(Solution solution)
        {
            try
            {
                if (solution != null)
                {
                    foreach (var project in solution.ProjectList)
                    {
                        if (project.Site != null && project.Site.SiteSettings == null)
                        {
                            project.Site.SiteSettings = new SiteSettings();
                        }
                    }
                }

                this.OnSolutionLoaded(solution);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        public void SaveSolution(Solution solution)
        {
            try
            {
                if (solution != null)
                {
                    string solutionFilePath = string.Format(@"{0}\{1}.ei", this.SolutionPath, solution.Name);

                    JsonConverter[] converters = { new SectionSerializer() };
                    var serializerSettings = new JsonSerializerSettings()
                    {
                        //Converters = converters,
                        TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto,
                        NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
                        Formatting = Newtonsoft.Json.Formatting.Indented
                    };

                    string jsonText = Newtonsoft.Json.JsonConvert.SerializeObject(solution, serializerSettings);

                    if (!File.Exists(solutionFilePath))
                    {
                        var writer = File.CreateText(solutionFilePath);
                        writer.Write(jsonText);

                        writer.Close();
                    }
                    else
                    {
                        File.WriteAllText(solutionFilePath, jsonText, Encoding.Unicode);
                    }

                    CustomMessageBox.Show(Localization.LocalizationUtility.Convert("Message"), Localization.LocalizationUtility.Convert("SaveSuccessMessage"));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void OnSolutionLoaded(Solution solution)
        {
            this.FixLostDirectories(solution);

            if (this.SolutionLoaded != null)
            {
                this.SolutionLoaded(this, solution);
            }

            foreach (var vmi in DaVinciModule.Current.ViewMenuItem.Children)
            {
                if (vmi is DockableViewMenuItem)
                {
                    if ((vmi as DockableViewMenuItem).ViewType.Equals(typeof(Views.DesignerView)))
                    {
                        var designerVMI = vmi as DockableViewMenuItem;
                        this.windowNavigationService.NavigateView(designerVMI);
                    }

                    if ((vmi as DockableViewMenuItem).ViewType.Equals(typeof(Views.SolutionExplorerView)))
                    {
                        var solutionExplorerVMI = vmi as DockableViewMenuItem;
                        this.windowNavigationService.NavigateView(solutionExplorerVMI);
                    }

                    if ((vmi as DockableViewMenuItem).ViewType.Equals(typeof(Views.WebBrowserView)))
                    {
                        var webBrowserVMI = vmi as DockableViewMenuItem;
                        this.windowNavigationService.NavigateView(webBrowserVMI);
                    }
                }
            }
        }

        public void RefreshWebSite()
        {
            if (this.CurrentSolution != null && this.CurrentSolution.ProjectList != null)
            {
                if (!Directory.Exists(this.TempFolderPath))
                {
                    Directory.CreateDirectory(this.TempFolderPath);
                }

                this.ClearFolder(this.TempFolderPath);

                var defaultProject = this.CurrentSolution.ProjectList.FirstOrDefault();
                if (defaultProject != null && defaultProject.Site != null)
                {
                    foreach (var page in defaultProject.Site.PageList)
                    {
                        string htmlCode = page.GenerateCode();
                        var writer = File.CreateText(this.TempFolderPath + page.Name.Trim().ToLower() + ".html");
                        writer.Write(htmlCode);
                        writer.Close();

                        this.eventAggregator.GetEvent<PageRefreshEvent>().Publish(page);
                    }
                }

                this.CopyImagesFolder(this.TempFolderPath);
                this.CopyOtherFiles(this.TempFolderPath);
            }
        }

        private void ClearFolder(string folderPath)
        {
            if (string.IsNullOrEmpty(folderPath) || !Directory.Exists(folderPath))
            {
                return;
            }

            DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);

            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                file.Delete();
            }

            foreach (DirectoryInfo dir in directoryInfo.GetDirectories())
            {
                dir.Delete(true);
            }
        }

        private void CopyImagesFolder(string folderPath)
        {
            string sourceFolderPath = this.SolutionPath + "\\img\\";
            string destFolderPath = folderPath + "img\\";

            if (Directory.Exists(sourceFolderPath))
            {
                if (!Directory.Exists(destFolderPath))
                {
                    Directory.CreateDirectory(destFolderPath);
                }

                foreach (string filePath in Directory.GetFiles(sourceFolderPath))
                {
                    string fileName = Path.GetFileName(filePath);
                    File.Copy(filePath, destFolderPath + fileName);
                }
            }
        }

        private void CopyOtherFiles(string folderPath)
        {
            if (string.IsNullOrEmpty(folderPath))
            {
                return;
            }

            var assembly = Assembly.GetExecutingAssembly();
            
            using (Stream stream = assembly.GetManifestResourceStream("Ertis.DaVinci.Resources.WebContent.WebResources.zip"))
            {
                this.Unzip(stream, this.TempFolderPath);
            }
        }

        /*
        private void UnzipResources(Stream stream)
        {
            using (ZipInputStream s = new ZipInputStream(stream))
            {
                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    using (FileStream streamWriter = File.Create(this.TempFolderPath + theEntry.Name))
                    {
                        int size = 2048;
                        byte[] data = new byte[2048];
                        while (true)
                        {
                            size = s.Read(data, 0, data.Length);
                            if (size > 0)
                            {
                                streamWriter.Write(data, 0, size);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }
        */

        private void Unzip(Stream zipFileStream, string outputPath)
        {
            using (ZipFile zipFile = new ZipFile(zipFileStream))
            {
                foreach (ZipEntry entry in zipFile)
                {
                    if (entry.IsFile)
                    {
                        Stream stream = zipFile.GetInputStream(entry);
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            String fullPath = entry.Name;
                            if (fullPath.Contains("/"))
                                fullPath = fullPath.Replace("/", "\\");

                            fullPath = outputPath + fullPath;
                            string directoryPath = Path.GetDirectoryName(fullPath);
                            if (!Directory.Exists(directoryPath))
                            {
                                Directory.CreateDirectory(directoryPath);
                            }

                            using (StreamWriter writer = File.CreateText(fullPath))
                            {
                                writer.Write(reader.ReadToEnd());
                            }
                        }
                    }
                }
            }
        }

        private void FixLostDirectories(Solution solution)
        {
            if (!Directory.Exists(this.SolutionPath + "\\img"))
            {
                Directory.CreateDirectory(this.SolutionPath + "\\img");
            }
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
