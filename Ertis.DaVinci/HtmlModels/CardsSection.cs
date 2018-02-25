using Ertis.DaVinci.Managers;
using Ertis.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.DaVinci.HtmlModels
{
    public class CardsSection : Section, IHtml
    {
        public ObservableRangeCollection<ProductCard> Cards { get; set; }

        public CardsSection() : base(SectionType.CardsSection)
        {
            this.Cards = new ObservableRangeCollection<ProductCard>();
        }

        public string GenerateCode()
        {
            string cardsCode = string.Empty;
            foreach (var card in this.Cards)
            {
                cardsCode += card.GenerateCode() + Environment.NewLine;
            }

            return TemplateManager.Current.CardsSectionTemplate
                .Replace("{CARDS}", cardsCode);
        }
    }
}
