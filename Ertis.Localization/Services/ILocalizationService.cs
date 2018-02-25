using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ertis.Localization.Services
{
    public interface ILocalizationService
    {
        void UpdateTranslationResources(string cultureCode);

        void UpdateTranslationResources(string cultureCode, HashSet<string> ignoredLocalizations);

        CultureInfo ChangeUICulture(string cultureCode);

        FlowDirection UiFlowDirection { get; }
    }
}
