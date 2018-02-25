using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

namespace Ertis.Shared.ScreenManagement
{
    public class WpfScreen
    {
        #region Fields

        private readonly Screen screen;
        private System.Windows.Point absoluteLocation = new System.Windows.Point(0, 0);

        #endregion

        #region Properties

        public static WpfScreen Primary
        {
            get { return new WpfScreen(System.Windows.Forms.Screen.PrimaryScreen); }
        }

        public Rect DeviceBounds
        {
            get { return this.GetRect(this.screen.Bounds); }
        }

        public Rect WorkingArea
        {
            get { return this.GetRect(this.screen.WorkingArea); }
        }

        public System.Windows.Size Resulotion
        {
            get
            {
                return this.DeviceBounds.Size;
            }
        }

        public bool IsPrimary
        {
            get { return this.screen.Primary; }
        }

        public string DeviceName
        {
            get { return this.screen.DeviceName; }
        }
        
        public System.Windows.Point AbsoluteLocation
        {
            get
            {
                return absoluteLocation;
            }

            set
            {
                absoluteLocation = value;
            }
        }

        #endregion

        #region Construcors

        /// <summary>
        /// Construcor 1
        /// </summary>
        /// <param name="screen"></param>
        internal WpfScreen(System.Windows.Forms.Screen screen)
        {
            this.screen = screen;
        }

        /// <summary>
        /// Construcor 2
        /// </summary>
        /// <param name="screen"></param>
        internal WpfScreen(Window window)
        {
            WindowInteropHelper windowInteropHelper = new WindowInteropHelper(window);
            this.screen = System.Windows.Forms.Screen.FromHandle(windowInteropHelper.Handle);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Mevcut tüm ekranlar
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<WpfScreen> AllScreens()
        {
            foreach (Screen screen in System.Windows.Forms.Screen.AllScreens)
            {
                yield return new WpfScreen(screen);
            }
        }

        /// <summary>
        /// Paremetredeki pencere hangi ekranda?
        /// </summary>
        /// <param name="window"></param>
        /// <returns></returns>
        public static WpfScreen GetScreenFrom(Window window)
        {
            WindowInteropHelper windowInteropHelper = new WindowInteropHelper(window);
            Screen screen = System.Windows.Forms.Screen.FromHandle(windowInteropHelper.Handle);
            WpfScreen wpfScreen = new WpfScreen(screen);
            return wpfScreen;
        }

        /// <summary>
        /// Paremetredeki point hangi ekranda?
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static WpfScreen GetScreenFrom(System.Windows.Point point)
        {
            int x = (int)Math.Round(point.X);
            int y = (int)Math.Round(point.Y);

            System.Drawing.Point drawingPoint = new System.Drawing.Point(x, y);
            Screen screen = System.Windows.Forms.Screen.FromPoint(drawingPoint);
            WpfScreen wpfScreen = new WpfScreen(screen);

            return wpfScreen;
        }

        private Rect GetRect(Rectangle value)
        {
            return new Rect
            {
                X = value.X,
                Y = value.Y,
                Width = value.Width,
                Height = value.Height
            };
        }

        public override bool Equals(object obj)
        {
            if (obj is WpfScreen)
            {
                var other = obj as WpfScreen;
                if (other.DeviceName == this.DeviceName)
                    return true;
                else return false;
            }
            else
                return false;
        }

        public override int GetHashCode()
        {
            return (int)CalculateHash(this.DeviceName);
        }

        private static UInt64 CalculateHash(string read)
        {
            UInt64 hashedValue = 3074457345618258791ul;
            for (int i = 0; i < read.Length; i++)
            {
                hashedValue += read[i];
                hashedValue *= 3074457345618258799ul;
            }
            return hashedValue;
        }

        #endregion
    }
}
