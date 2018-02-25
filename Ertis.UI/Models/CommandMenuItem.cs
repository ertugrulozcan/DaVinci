using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ertis.Shared.Models
{
    public class CommandMenuItem : ViewMenuItemBase
    {
        public override void Navigate()
        {
            throw new NotImplementedException();
        }

        public override void Navigate<T>(T parameters)
        {
            throw new NotImplementedException();
        }

        public CommandMenuItem(int id, string title, string iconKey, ICommand command) : base(id, title)
        {
            this.IconKey = iconKey;
            this.Command = command;
        }
    }
}
