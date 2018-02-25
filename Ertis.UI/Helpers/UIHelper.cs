using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Ertis.Shared.Helpers
{
    public class UIHelper
    {
        public static void UiInvoke(DispatcherPriority dispatcherPriority, bool isAsync, Action a)
        {
            // for tests just invoke
            if (Application.Current == null)
                a.Invoke();
            else if (!isAsync)
                Application.Current.Dispatcher.Invoke(a, dispatcherPriority);
            else
                Application.Current.Dispatcher.BeginInvoke(a, dispatcherPriority);
        }

        public static void Background(Action a)
        {
            if (Application.Current == null)
                a.Invoke();
            else
                Application.Current.Dispatcher.Invoke(a, DispatcherPriority.Background);
        }
    }
}
