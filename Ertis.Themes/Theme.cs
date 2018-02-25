using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ertis.Themes
{
    public class Theme
    {
        #region Properties

        public string Name { get; private set; }

        public string Key { get; private set; }

        public Uri ResourcePath { get; private set; }

        public ResourceDictionary Resources { get; private set; }
        
        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="key">Temanın adı</param>
        /// <param name="path">Temanın resource yolu</param>
        public Theme(string name, string key, string path)
        {
            this.Name = name;
            this.Key = key;
            this.ResourcePath = new Uri(path, UriKind.RelativeOrAbsolute);
            if (Application.Current.Resources.MergedDictionaries.Any(x => x.Source.AbsolutePath.Contains(this.Key)))
                this.Resources = Application.Current.Resources.MergedDictionaries.First(x => x.Source.AbsolutePath.Contains(this.Key));
            else
            {
                this.Resources = new ResourceDictionary()
                {
                    Source = this.ResourcePath
                };

                Application.Current.Resources.MergedDictionaries.Add(this.Resources);
            }
        }

        #endregion
    }

    public class ThemeException : Exception
    {
        public ThemeException(string message) : base(message)
        {
            System.Diagnostics.Debug.WriteLine(message);
        }
    }
}
