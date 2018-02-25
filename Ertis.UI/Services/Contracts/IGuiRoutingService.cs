using Ertis.Shared.Models;
using Ertis.Shared.Routing.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.Shared.Services.Contracts
{
    public interface IGuiRoutingService
    {
        void Register(IGuiRoutableModule guiRoutableModule);

        ReadOnlyObservableCollection<IViewMenuItem> MainViewMenuItems { get; }

        ReadOnlyObservableCollection<SettingsViewMenuItem> SettingsViewMenuItems { get; }

        void RegisterTopMenuPresenterModule(ITopMenuPresenter presenterModule);

        ITopMenuPresenter TopMenuPresenterModule { get; }
    }
}
