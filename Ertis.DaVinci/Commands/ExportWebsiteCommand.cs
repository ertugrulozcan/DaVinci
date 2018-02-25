using Ertis.DaVinci.Services.Interfaces;
using Ertis.Shared.Components;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.DaVinci.Commands
{
    public class ExportWebsiteCommand : DelegateCommand
    {
        private static readonly Action ExecuteAction = delegate
        {
            try
            {
                var solutionService = ServiceLocator.Current.GetInstance<ISolutionService>();

                if (solutionService == null || solutionService.CurrentSolution == null || solutionService.TempFolderPath == null)
                    return;

                if (!Directory.Exists(solutionService.TempFolderPath))
                    return;

                if (Directory.GetFiles(solutionService.TempFolderPath).Length == 0)
                    return;

                FixDamagedFontFiles(solutionService.TempFolderPath);

                using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
                {
                    System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                    if (result == System.Windows.Forms.DialogResult.OK)
                    {
                        ZipFolder(dialog.SelectedPath + "\\" + solutionService.CurrentSolution.Name + ".zip", solutionService.TempFolderPath);
                    }
                }

                CustomMessageBox.Show(Localization.LocalizationUtility.Convert("Message"), Localization.LocalizationUtility.Convert("ExportSuccessMessage"));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
           
        };

        public ExportWebsiteCommand() : base(ExecuteAction)
        {

        }

        private static void ZipFolder(string outPathname, string folderName, string password = null)
        {
            FileStream fsOut = File.Create(outPathname);
            ZipOutputStream zipStream = new ZipOutputStream(fsOut);

            // 0-9, 9 being the highest level of compression
            zipStream.SetLevel(3);

            // optional. Null is the same as not setting. Required if using AES.
            zipStream.Password = password;
            
            int folderOffset = folderName.Length + (folderName.EndsWith("\\") ? 0 : 1);

            CompressFolder(folderName, zipStream, folderOffset);

            zipStream.IsStreamOwner = true;
            zipStream.Close();
        }

        private static void CompressFolder(string path, ZipOutputStream zipStream, int folderOffset)
        {
            string[] files = Directory.GetFiles(path);

            foreach (string filename in files)
            {

                FileInfo fi = new FileInfo(filename);

                string entryName = filename.Substring(folderOffset);
                entryName = ZipEntry.CleanName(entryName);
                ZipEntry newEntry = new ZipEntry(entryName);
                newEntry.DateTime = fi.LastWriteTime;
                
                newEntry.Size = fi.Length;

                zipStream.PutNextEntry(newEntry);
                
                byte[] buffer = new byte[4096];
                using (FileStream streamReader = File.OpenRead(filename))
                {
                    StreamUtils.Copy(streamReader, zipStream, buffer);
                }

                zipStream.CloseEntry();
            }

            string[] folders = Directory.GetDirectories(path);
            foreach (string folder in folders)
            {
                CompressFolder(folder, zipStream, folderOffset);
            }
        }

        private static void FixDamagedFontFiles(string outputPath)
        {
            // Damaged font files;
            // \fonts\lightcase.eot
            // \fonts\lightcase.ttf
            // \fonts\lightcase.woff
            // \libs\revolution\fonts\revicons\revicons.ttf
            // \libs\revolution\fonts\revicons\revicons.woff
            // \libs\ionicons-2.0.1\fonts\ionicons.ttf
            // \libs\ionicons-2.0.1\fonts\ionicons.woff

            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                string resourcesPath = "Ertis.DaVinci.Resources.Others";
                Dictionary<string, string> damagedFileNames = new Dictionary<string, string>()
                {
                    { "lightcase.eot", "\\fonts\\lightcase.eot" },
                    { "lightcase.ttf", "\\fonts\\lightcase.ttf" },
                    { "lightcase.woff", "\\fonts\\lightcase.woff" },
                    { "revicons.ttf", "\\libs\\revolution\\fonts\\revicons\\revicons.ttf" },
                    { "revicons.woff", "\\libs\\revolution\\fonts\\revicons\\revicons.woff" },
                    { "ionicons.ttf", "\\libs\\ionicons-2.0.1\\fonts\\ionicons.ttf" },
                    { "ionicons.woff", "\\libs\\ionicons-2.0.1\\fonts\\ionicons.woff" },
                    { "fontawesome-webfont.woff", "\\libs\\font-awesome-4.7.0\\fonts\\fontawesome-webfont.woff" },
                    { "fontawesome-webfont.woff2", "\\libs\\font-awesome-4.7.0\\fonts\\fontawesome-webfont.woff2" },
                    { "fontawesome-webfont.ttf", "\\libs\\font-awesome-4.7.0\\fonts\\fontawesome-webfont.ttf" }
                };

                foreach (var file in damagedFileNames)
                {
                    string manifestPath = string.Format("{0}.{1}", resourcesPath, file.Key);
                    using (Stream stream = assembly.GetManifestResourceStream(manifestPath))
                    {
                        File.WriteAllBytes(outputPath + file.Value, StreamToByteArray(stream));
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ExportWebsiteCommand.FixDamagedFontFiles() error! : " + ex.Message);
            }
        }

        private static byte[] StreamToByteArray(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }

                return ms.ToArray();
            }
        }
    }
}
