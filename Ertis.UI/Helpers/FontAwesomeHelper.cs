using Ertis.Shared.Models;
using FontAwesome.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Ertis.Shared.Helpers
{
    public static class FontAwesomeHelper
    {
        public static Dictionary<string, FontAwesomeIcon> IconNameDictionary { get; private set; }

        public static Dictionary<int, FontAwesomeIcon> IconValueDictionary { get; private set; }

        public static SolidColorBrush defaultIconBrush = new SolidColorBrush(Color.FromRgb(0xFF, 0xFF, 0xFF));

        public static SolidColorBrush DefaultIconBrush
        {
            get
            {
                var brush = Themes.ThemeManager.Current.GetElementFromResource("ConstractBrush2") as SolidColorBrush;
                if (brush == null)
                    return defaultIconBrush;
                else
                    return brush;
            }
        }

        static FontAwesomeHelper()
        {
            PrepareIconDictionary();
        }

        private static void PrepareIconDictionary()
        {
            IconValueDictionary = new Dictionary<int, FontAwesomeIcon>();
            IconNameDictionary = new Dictionary<string, FontAwesomeIcon>();
            
            foreach (FontAwesomeIcon icon in Enum.GetValues(typeof(FontAwesomeIcon)))
            {
                if (!IconValueDictionary.ContainsKey((int)icon))
                {
                    IconValueDictionary.Add((int)icon, icon);
                    string name = Enum.GetName(typeof(FontAwesomeIcon), icon);
                    IconNameDictionary.Add(name, icon);
                }
            }
        }

        public static ImageAwesome GetDefaultIconByVmiType(ViewMenuItemBase item)
        {
            var icon = new ImageAwesome();

            SetDefaultIconProperties(icon);

            return icon;
        }

        private static void SetDefaultIconProperties(ImageAwesome icon)
        {
            icon.Icon = FontAwesomeIcon.CaretRight;
            icon.Width = 10;
            icon.Height = 10;
            icon.Foreground = DefaultIconBrush;
        }
    }
}
