using Ertis.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.Main.ViewModels
{
    public class FooterBarViewModel : BaseViewModel
    {
        #region Constructors

        public FooterBarViewModel() : base(Guid.NewGuid().ToString())
        {

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
    }
}
