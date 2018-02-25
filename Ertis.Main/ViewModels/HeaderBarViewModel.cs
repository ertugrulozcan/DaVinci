using Ertis.Shared.Models;
using Ertis.Shared.Services.Contracts;
using Ertis.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.Main.ViewModels
{
    public class HeaderBarViewModel : BaseViewModel
    {
        #region Services

        private readonly IGuiRoutingService guiRoutingService;

        #endregion

        #region Fields

        private List<IViewMenuItem> topMenuVmiList;

        #endregion

        #region Properties

        public List<IViewMenuItem> TopMenuVmiList
        {
            get
            {
                return topMenuVmiList;
            }

            set
            {
                this.topMenuVmiList = value;
                this.RaisePropertyChanged("TopMenuVmiList");
            }
        }

        #endregion

        #region Constructors

        public HeaderBarViewModel(IGuiRoutingService guiRoutingService) : base(Guid.NewGuid().ToString())
        {
            this.guiRoutingService = guiRoutingService;

            this.Initialize();
        }

        #endregion

        #region Navigation Methods
        
        protected override void OnNavigatedTo(object parameter)
        {

        }

        protected override void OnNavigatedFrom()
        {

        }

        #endregion

        #region Methods

        private void Initialize()
        {
            this.GenerateTopMenuVMIs();
        }

        private void GenerateTopMenuVMIs()
        {
            this.TopMenuVmiList = new List<IViewMenuItem>();
            this.TopMenuVmiList.AddRange(guiRoutingService.TopMenuPresenterModule.GetTopMenuVmiList());
        }

        #endregion
    }
}
