using Ertis.DaVinci.HtmlModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Ertis.DaVinci.Converters
{
    public class SectionTemplateSelector : DataTemplateSelector
    {
        public DataTemplate BannerTemplate { get; set; }

        public DataTemplate BasicSectionTemplate { get; set; }

        public DataTemplate ImageSectionTemplate { get; set; }

        public DataTemplate ParagraphSectionTemplate { get; set; }

        public DataTemplate CardsSectionTemplate { get; set; }

        public DataTemplate BlogSectionTemplate { get; set; }

        public DataTemplate MetroSectionTemplate { get; set; }

        public DataTemplate GallerySectionTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is Section)
            {
                if ((item as Section).Type == SectionType.BannerSection)
                    return this.BannerTemplate;
                if ((item as Section).Type == SectionType.BasicSection)
                    return this.BasicSectionTemplate;
                if ((item as Section).Type == SectionType.CardsSection)
                    return this.CardsSectionTemplate;
                if ((item as Section).Type == SectionType.ImageSection)
                    return this.ImageSectionTemplate;
                if ((item as Section).Type == SectionType.ParagraphSection)
                    return this.ParagraphSectionTemplate;
                if ((item as Section).Type == SectionType.BlogSection)
                    return this.BlogSectionTemplate;
                if ((item as Section).Type == SectionType.MetroSection)
                    return this.MetroSectionTemplate;
                if ((item as Section).Type == SectionType.Gallery)
                    return this.GallerySectionTemplate;
            }

            return null;
        }
    }
}
