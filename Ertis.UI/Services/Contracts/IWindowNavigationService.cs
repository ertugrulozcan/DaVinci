using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ertis.Shared.Models;

namespace Ertis.Shared.Services.Contracts
{
    public interface IWindowNavigationService
    {
        NavigationWrapperType NavigationMode { get; }

        void NavigateView(ViewMenuItemBase vmi);

        void NavigateView(ViewMenuItemBase vmi, object parameter);

        object GetNavigationParameter(string viewName);
    }
}
