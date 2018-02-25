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
    public abstract class ViewMenuItemBase : IViewMenuItem, ISearchable
    {
        #region Fields

        private int id;
        private Type viewType;
        private string title;
        private string shortTitle;
        private string localizationKey;
        private string iconKey;
        private ImageAwesome fontAwesomeIcon;
        private ErtAwesome.ImageIcon vectorAwesomeIcon;
        private BitmapSource imageIconSource;
        private VectorRendering awesomeType;
        private ICommand command;
        private IViewMenuItem parent;
        private List<IViewMenuItem> children;

        #endregion

        #region Properties

        public int Id
        {
            get
            {
                return this.id;
            }

            private set
            {
                this.id = value;
            }
        }

        public string ViewName
        {
            get
            {
                if (this.ViewType == null)
                    return null;

                return this.ViewType.FullName;
            }
        }

        public Type ViewType
        {
            get
            {
                return viewType;
            }

            private set
            {
                this.viewType = value;
            }
        }

        public string Title
        {
            get
            {
                if (string.IsNullOrEmpty(this.LocalizationKey))
                    return title;
                else
                    return title; // Localize(this.Title);
            }

            private set
            {
                this.title = value;
            }
        }

        public string ShortTitle
        {
            get
            {
                if (string.IsNullOrEmpty(this.shortTitle))
                    return title;
                else
                    return shortTitle;
            }

            set
            {
                this.shortTitle = value;
            }
        }

        public string LocalizationKey
        {
            get
            {
                return localizationKey;
            }

            set
            {
                this.localizationKey = value;
            }
        }

        public string IconColor { get; set; }

        public BitmapSource ImageIconSource
        {
            get
            {
                return imageIconSource;
            }

            protected set
            {
                imageIconSource = value;
            }
        }

        public string IconKey
        {
            get
            {
                return iconKey;
            }

            protected set
            {
                this.iconKey = value;

                if (!string.IsNullOrEmpty(value))
                {
                    if (FontAwesomeHelper.IconNameDictionary.ContainsKey(value))
                    {
                        var foregroundColor = GetColor(false);
                        this.FontAwesomeIcon = new ImageAwesome() { Icon = FontAwesomeHelper.IconNameDictionary[value], Foreground = foregroundColor };
                        this.AwesomeType = VectorRendering.FontAwesome;
                    }
                    else if (ErtAwesome.IconManager.Current.ContainsByName(value))
                    {
                        this.VectorAwesomeIcon = ErtAwesome.IconManager.Current.GetIconByName(value, false);
                        Brush fill = GetColor(true);
                        if (fill != null)
                            this.VectorAwesomeIcon.Fill = fill;
                        if (this.VectorAwesomeIcon.Fill == null)
                            this.VectorAwesomeIcon.Fill = FontAwesomeHelper.DefaultIconBrush;

                        this.AwesomeType = VectorRendering.ErtAwesome;
                    }
                    else
                    {
                        try
                        {
                            var bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.UriSource = new Uri(value);
                            bitmap.CreateOptions = BitmapCreateOptions.DelayCreation;
                            bitmap.EndInit();
                            this.ImageIconSource = bitmap;

                            //this.ImageIconSource = new BitmapImage(new Uri(value, UriKind.RelativeOrAbsolute));
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine(value + " icon dosyası bulunamadı!");
                        }
                    }
                }
            }
        }

        public ImageAwesome FontAwesomeIcon
        {
            get
            {
                if (this.fontAwesomeIcon == null)
                    return FontAwesomeHelper.GetDefaultIconByVmiType(this);

                return this.fontAwesomeIcon;
            }

            set
            {
                this.fontAwesomeIcon = value;
            }
        }

        public ErtAwesome.ImageIcon VectorAwesomeIcon
        {
            get
            {
                if (this.vectorAwesomeIcon == null)
                    return new ErtAwesome.ImageIcon() { Icon = ErtAwesome.IconCollection.None };

                return this.vectorAwesomeIcon;
            }

            set
            {
                this.vectorAwesomeIcon = value;
            }
        }

        public object AwesomeIcon
        {
            get
            {
                if (this.AwesomeType == VectorRendering.FontAwesome)
                    return this.FontAwesomeIcon;
                else
                    return this.VectorAwesomeIcon;
            }
        }

        public VectorRendering AwesomeType
        {
            get
            {
                return this.awesomeType;
            }

            protected set
            {
                this.awesomeType = value;
            }
        }

        public ICommand Command
        {
            get
            {
                return this.command;
            }

            protected set
            {
                this.command = value;
            }
        }

        public IViewMenuItem Parent
        {
            get
            {
                return parent;
            }

            set
            {
                this.parent = value;
            }
        }

        public List<IViewMenuItem> Children
        {
            get
            {
                return children;
            }

            private set
            {
                this.children = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor for parent modules
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        protected ViewMenuItemBase(int id, string title)
        {
            this.Id = id;
            this.Title = title;
            this.Children = new List<IViewMenuItem>();
        }

        protected ViewMenuItemBase(int id, Type viewType, string title)
        {
            this.Id = id;
            this.Title = title;
            this.ViewType = viewType;
            this.Children = new List<IViewMenuItem>();
        }

        #endregion

        #region Methods

        public abstract void Navigate();

        public abstract void Navigate<T>(T parameters);

        private Brush GetColor(bool isOverwrite)
        {
            if (string.IsNullOrEmpty(IconColor))
                return isOverwrite ? null : FontAwesomeHelper.DefaultIconBrush;
            else
            {
                try
                {
                    Color color = (Color)ColorConverter.ConvertFromString("#" + IconColor);
                    return new SolidColorBrush(color);
                }
                catch (Exception ex)
                {
                    return isOverwrite ? null : FontAwesomeHelper.DefaultIconBrush;
                }
            }
        }

        public override string ToString()
        {
            return this.Title;
        }

        #endregion

        #region ISearchable

        public SearchResultBadge CategoryBadge
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string SearchKey
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ICommand SearchResultSelectCommand
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Enums

        public enum VectorRendering
        {
            FontAwesome,
            ErtAwesome
        }

        #endregion
    }
}
