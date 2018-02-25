using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFLocalizeExtension.Extensions;

namespace Ertis.Localization
{
    public static class LocalizationUtility
    {
        public static string Convert(string key)
        {
            string uiString = (key).Replace(" ", "");
            LocExtension locExtension = new LocExtension(key);
            locExtension.ResolveLocalizedValue(out uiString);
            return uiString;
        }
    }
}
