using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ertis.Shared.ScreenManagement
{
    public class WpfScreenManager
    {
        #region Fields

        private readonly Window window;
        private WpfScreen currentScreen;
        private IList<WpfScreen> screenList;

        public delegate void CurrentScreenChangeEvent(CurrentScreenChangeEventArgs args);
        public event CurrentScreenChangeEvent OnCurrentScreenChanged;
        public event EventHandler DisplaySettingsChanged;

        #endregion

        #region Properties

        public static WpfScreenManager Current { get; private set; }

        /// <summary>
        /// Programın üzerinde çalıştığı ekran;
        /// </summary>
        public WpfScreen CurrentScreen
        {
            get
            {
                return this.currentScreen;
            }
            set
            {
                this.PreviousWidth = value.DeviceBounds.Width;
                this.PreviousHeight = value.DeviceBounds.Height;

                this.currentScreen = value;
            }
        }

        public IList<WpfScreen> ScreenList
        {
            get
            {
                return screenList;
            }

            set
            {
                screenList = value;
            }
        }

        public double PreviousWidth { get; set; }
        public double PreviousHeight { get; set; }

        #endregion

        #region Construcors

        public WpfScreenManager(Window window)
        {
            this.window = window;
            this.OnCurrentScreenChanged += (args) => { System.Diagnostics.Debug.WriteLine("Primary screen changed : " + args.NewScreen.DeviceName); };
            Microsoft.Win32.SystemEvents.DisplaySettingsChanged += SystemEvents_DisplaySettingsChanged;

            this.ScreenList = WpfScreen.AllScreens().OrderBy(x => x.DeviceBounds.X).ToList();
            this.UpdateCurrentScreen();

            Current = this;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Pencere taşındığında vs. çağırılmalıdır.
        /// </summary>
        public void UpdateCurrentScreen()
        {
            if (this.window == null)
                return;

            var oldScreen = this.CurrentScreen;
            this.CurrentScreen = new WpfScreen(this.window);

            if (oldScreen != null)
            {
                double absoluteX = oldScreen.WorkingArea.Left - oldScreen.WorkingArea.X;
                double absoluteY = oldScreen.WorkingArea.Top - oldScreen.WorkingArea.Y;
                this.CurrentScreen.AbsoluteLocation = new Point(absoluteX, absoluteY);
            }

            if (!this.CurrentScreen.Equals(oldScreen))
            {
                this.OnCurrentScreenChanged(new CurrentScreenChangeEventArgs(oldScreen, this.CurrentScreen));
            }
        }

        void SystemEvents_DisplaySettingsChanged(object sender, EventArgs e)
        {
            this.ScreenList = WpfScreen.AllScreens().OrderBy(x => x.DeviceBounds.X).ToList();
            this.UpdateCurrentScreen();

            var handler = this.DisplaySettingsChanged;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }

        #endregion
    }

    public class CurrentScreenChangeEventArgs
    {
        public WpfScreen NewScreen { get; private set; }
        public WpfScreen OldScreen { get; private set; }

        public CurrentScreenChangeEventArgs(WpfScreen oldScreen, WpfScreen newScreen)
        {
            this.OldScreen = oldScreen;
            this.NewScreen = newScreen;
        }
    }
}
