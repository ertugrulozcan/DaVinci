using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.DaVinci.Managers
{
    public class TemplateManager
    {
        #region Constants

        private readonly string AssemblyNamespace = "Ertis.DaVinci.Resources.Templates";

        #endregion

        #region Fields

        private static TemplateManager current_;

        #endregion

        #region Properties

        public static TemplateManager Current
        {
            get
            {
                if (current_ == null)
                    current_ = new TemplateManager();

                return current_;
            }
        }

        public string PageTemplate { get; private set; }
        public string HeadTemplate { get; private set; }
        public string BodyTemplate { get; private set; }
        public string HeaderTemplate { get; private set; }
        public string MainTemplate { get; private set; }
        public string FooterTemplate { get; private set; }

        public string BannerTemplate { get; private set; }
        public string BannerSlideTemplate { get; private set; }
        public string BasicSectionTemplate { get; private set; }
        public string CardsSectionTemplate { get; private set; }
        public string ProductCardTemplate { get; private set; }
        public string ParagraphSectionTemplate { get; private set; }
        public string ImageSectionTemplate { get; private set; }
        public string ImageBoxTemplate { get; private set; }
        public string MetroTemplate { get; private set; }
        public string BlogTemplate { get; private set; }
        public string ContactTemplate { get; private set; }
        public string GoogleMapTemplate { get; private set; }
        public string GalleryTemplate { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        private TemplateManager()
        {
            this.SetTemplates();
        }

        #endregion

        #region Methods

        private void SetTemplates()
        {
            this.PageTemplate = this.GetTemplateFromResource("page.html");
            this.HeadTemplate = this.GetTemplateFromResource("head.html");
            this.BodyTemplate = this.GetTemplateFromResource("body.html");
            this.MainTemplate = this.GetTemplateFromResource("main.html");
            this.HeaderTemplate = this.GetTemplateFromResource("header.html");
            this.FooterTemplate = this.GetTemplateFromResource("footer.html");
            this.BannerTemplate = this.GetTemplateFromResource("sections.banner.html");
            this.BannerSlideTemplate = this.GetTemplateFromResource("banner.slide.html");
            this.BasicSectionTemplate = this.GetTemplateFromResource("sections.basic.html");
            this.CardsSectionTemplate = this.GetTemplateFromResource("sections.cards.html");
            this.ProductCardTemplate = this.GetTemplateFromResource("product.card.html");
            this.ParagraphSectionTemplate = this.GetTemplateFromResource("sections.paragraph.html");
            this.ImageSectionTemplate = this.GetTemplateFromResource("sections.image.html");
            this.ImageBoxTemplate = this.GetTemplateFromResource("imagebox.html");
            this.MetroTemplate = this.GetTemplateFromResource("sections.metro.html");
            this.BlogTemplate = this.GetTemplateFromResource("sections.blog.html");
            this.ContactTemplate = this.GetTemplateFromResource("sections.contact.html");
            this.GoogleMapTemplate = this.GetTemplateFromResource("sections.googlemap.html");
            this.GalleryTemplate = this.GetTemplateFromResource("sections.gallery.html");
        }

        private string GetTemplateFromResource(string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            string code;
            using (Stream stream = assembly.GetManifestResourceStream(this.AssemblyNamespace + "." + fileName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    code = reader.ReadToEnd();
                }
            }

            return code;
        }

        #endregion
    }
}
