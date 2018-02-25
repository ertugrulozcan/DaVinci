using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ErtAwesome
{
    public class ImageIcon : Viewbox
    {
        #region Fields



        #endregion

        #region Properties

        private Path CanvasPath { get; set; }

        #endregion

        #region Dependency Properties

        public IconCollection Icon
        {
            get { return (IconCollection)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Icon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(IconCollection), typeof(ImageIcon), new PropertyMetadata(IconCollection.None, IconChangedCallback));


        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Fill.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FillProperty =
            DependencyProperty.Register("Fill", typeof(Brush), typeof(ImageIcon), new PropertyMetadata(null, FillChangedCallback));
        

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public ImageIcon()
        {
            this.Stretch = Stretch.Uniform;
            this.CanvasPath = new Path()
            {
                Stretch = Stretch.Fill,
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Center,
            };

            Binding binding = new Binding();
            binding.Source = this.Width;
            this.CanvasPath.SetBinding(Path.WidthProperty, binding);

            binding = new Binding();
            binding.Source = this.Height;
            this.CanvasPath.SetBinding(Path.HeightProperty, binding);

            binding = new Binding();
            binding.Source = this.Fill;
            this.CanvasPath.SetBinding(Path.FillProperty, binding);

            this.Child = this.CanvasPath;
        }

        #endregion

        #region Callback Methods

        private static void IconChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as ImageIcon;

            VectorData vector = IconManager.Current.GetVectorData(self.Icon);
            if (vector != null)
            {
                self.CanvasPath.Data = System.Windows.Media.Geometry.Parse(vector.Data);
                if (vector.Fill != null)
                    self.Fill = vector.Fill;
            }
        }

        private static void FillChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as ImageIcon;
            self.CanvasPath.Fill = self.Fill;
        }

        #endregion
    }
}
