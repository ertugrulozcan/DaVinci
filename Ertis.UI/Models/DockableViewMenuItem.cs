using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.Shared.Models
{
    public class DockableViewMenuItem : ViewMenuItemBase
    {
        #region Constructors

        /// <summary>
        /// Constructor for parent modules
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="iconKey"></param>
        public DockableViewMenuItem(int id, string title, string iconKey) : base(id, title)
        {
            this.IconKey = iconKey;
        }

        /// <summary>
        /// Constructor for child views
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="iconKey"></param>
        public DockableViewMenuItem(int id, Type viewType, string title, string iconKey) : base(id, viewType, title)
        {
            this.IconKey = iconKey;

            if (!string.IsNullOrEmpty(this.ViewName))
                this.Command = new Commands.DockViewCommand(this);
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
