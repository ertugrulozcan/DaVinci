using DevExpress.Xpf.Layout.Core;
using Ertis.Themes.Events;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ertis.Themes
{
    public class ThemeManager
    {
        #region Fields

        private static ThemeManager self;

        private Theme currentTheme;

        #endregion

        #region Properties

        public static ThemeManager Current
        {
            get
            {
                if (self == null)
                    self = new ThemeManager();

                return self;
            }
        }

        public List<Theme> Themes { get; private set; }

        public Theme CurrentTheme
        {
            get
            {
                return currentTheme;
            }
            private set
            {
                currentTheme = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        private ThemeManager()
        {
            this.Initialize();
        }

        #endregion

        #region Events

        public event EventHandler<Theme> ThemeChanged;

        #endregion
        
        #region Methods

        private void Initialize()
        {
            this.Themes = new List<Theme>();

            this.Themes.Add(new Theme("Dark", "ErtisDark", "pack://application:,,,/Ertis.Themes;component/Themes/ErtisDark.xaml"));
            this.Themes.Add(new Theme("Light", "ErtisLight", "pack://application:,,,/Ertis.Themes;component/Themes/ErtisLight.xaml"));
        }

        public object GetElementFromResource(string key)
        {
            if (this.CurrentTheme != null)
                return this.CurrentTheme.Resources[key];
            else
                return null;
        }

        public void ChangeTheme(string key)
        {
            if (this.Themes.Any(x => x.Key == key))
            {
                var theme = Themes.First(x => x.Key == key);
                Application.Current.Resources.MergedDictionaries.Clear();
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
                {
                    Source = theme.ResourcePath
                });

                if (theme.Key.Contains("Light"))
                {
                    DevExpress.Xpf.Core.ApplicationThemeHelper.ApplicationThemeName = DevExpress.Xpf.Core.Theme.MetropolisLightName;

                    Telerik.Windows.Controls.StyleManager.ApplicationTheme = new Telerik.Windows.Controls.Windows8Theme(); //Telerik
                    // SciChart.Charting.ThemeManager.DefaultTheme = "ExpressionLight";
                }
                else
                {
                    DevExpress.Xpf.Core.ApplicationThemeHelper.ApplicationThemeName = DevExpress.Xpf.Core.Theme.MetropolisDarkName;

                    Telerik.Windows.Controls.StyleManager.ApplicationTheme = new Telerik.Windows.Controls.Expression_DarkTheme(); // Telerik
                    // SciChart.Charting.ThemeManager.DefaultTheme = "SciChartv4Dark";
                }

                this.CurrentTheme = theme;

                if (this.CurrentTheme != null)
                {
                    var eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
                    eventAggregator.GetEvent<ThemeChangedEvent>().Publish(this.CurrentTheme);

                    if (this.ThemeChanged != null)
                    {
                        this.ThemeChanged(this, this.CurrentTheme);
                    }
                }
            }
            else
            {
                throw new Ertis.Themes.ThemeException("CurrentTheme olarak verilen key temalar arasında bulunamadı! (" + key + ")");
            }
        }

        #endregion
    }
}
