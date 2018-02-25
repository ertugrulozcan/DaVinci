using Ertis.Shared.Helpers;
using Ertis.Shared.Search;
using FontAwesome.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Ertis.Shared.Models
{
    public class ViewMenuItem : ViewMenuItemBase
    {
        #region Constructors

        /// <summary>
        /// Constructor for parent modules
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="iconKey"></param>
        public ViewMenuItem(int id, string title, string iconKey) : base(id, title)
        {
            this.IconKey = iconKey;
        }

        /// <summary>
        /// Constructor for child views
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="iconKey"></param>
        public ViewMenuItem(int id, Type viewType, string title, string iconKey) : base(id, viewType, title)
        {
            this.IconKey = iconKey;

            if (!string.IsNullOrEmpty(this.ViewName))
                this.Command = new Commands.OpenViewCommand(this);
        }

        #endregion

        #region Methods

        public override void Navigate()
        {
            if (this.Command != null && this.Command.CanExecute(null))
            {
                this.Command.Execute(null);
            }
        }

        public override void Navigate<T>(T parameters)
        {
            if (this.Command != null && this.Command.CanExecute(parameters))
            {
                this.Command.Execute(parameters);
            }
        }

        #endregion
    }
}
