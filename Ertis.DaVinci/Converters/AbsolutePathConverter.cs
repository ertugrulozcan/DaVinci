using Ertis.DaVinci.Managers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Ertis.DaVinci.Converters
{
    public class AbsolutePathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || SolutionManager.Current.CurrentSolution == null)
                return string.Empty;

            string absolutePath = string.Format("{0}\\{1}", SolutionManager.Current.CurrentSolution.SolutionPath, value.ToString());
            return absolutePath;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
