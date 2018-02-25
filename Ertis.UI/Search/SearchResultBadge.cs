using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Ertis.Shared.Search
{
    public class SearchResultBadge
    {
        public enum Badges
        {
            UnderlyingDefinition = 8,
            Symbol = 4,
            Chart = 1,
            Grid = 2,
            View = 3,
            EnergySymbol = 7,
            Workspace = 5,
            News = 6,
            DidYouMean = 10,
            Command = 9,
            OnlineSearch = 11
        }


        private string caption;
        private Brush borderBrush;

        public string Caption
        {
            get
            {
                return caption;
            }

            private set
            {
                this.caption = value;
            }
        }

        public Brush BorderBrush
        {
            get
            {
                return borderBrush;
            }

            private set
            {
                this.borderBrush = value;
            }
        }

        private Badges currentBadge;
        public Badges CurrentBadge { get { return currentBadge; } }

        private SearchResultBadge()
        {

        }

        public static SearchResultBadge Create(Badges badge)
        {
            switch (badge)
            {
                case Badges.UnderlyingDefinition:
                    return new SearchResultBadge()
                    {
                        Caption = "Contract",
                        BorderBrush = new SolidColorBrush(Color.FromRgb(0x8E, 0xC1, 0x47)),
                        currentBadge = badge
                    };
                case Badges.Symbol:
                    return new SearchResultBadge()
                    {
                        Caption = "Symbol",
                        BorderBrush = new SolidColorBrush(Color.FromRgb(0xC4, 0x29, 0x45)),
                        currentBadge = badge
                    };
                case Badges.Chart:
                    return new SearchResultBadge()
                    {
                        Caption = "Chart_",
                        BorderBrush = new SolidColorBrush(Color.FromRgb(0x36, 0xE6, 0x16)),
                        currentBadge = badge
                    };
                case Badges.Grid:
                    return new SearchResultBadge()
                    {
                        Caption = "Grid_",
                        BorderBrush = new SolidColorBrush(Color.FromRgb(0xFF, 0xEB, 0x38)),
                        currentBadge = badge
                    };
                case Badges.View:
                    return new SearchResultBadge()
                    {
                        Caption = "View",
                        BorderBrush = new SolidColorBrush(Color.FromRgb(0x1C, 0xB2, 0xED)),
                        currentBadge = badge
                    };
                case Badges.EnergySymbol:
                    return new SearchResultBadge()
                    {
                        Caption = "FinSymbol",
                        BorderBrush = new SolidColorBrush(Color.FromRgb(0x00, 0xFF, 0xFA)),
                        currentBadge = badge
                    };
                case Badges.Workspace:
                    return new SearchResultBadge()
                    {
                        Caption = "Workspace",
                        BorderBrush = new SolidColorBrush(Color.FromRgb(0xEE, 0xEE, 0xEE)),
                        currentBadge = badge
                    };
                case Badges.News:
                    return new SearchResultBadge()
                    {
                        Caption = "News_",
                        BorderBrush = new SolidColorBrush(Color.FromRgb(0xFF, 0x78, 0x13)),
                        currentBadge = badge
                    };
                case Badges.DidYouMean:
                    return new SearchResultBadge()
                    {
                        Caption = "?",
                        BorderBrush = new SolidColorBrush(Colors.Transparent),
                        currentBadge = badge
                    };
                case Badges.Command:
                    return new SearchResultBadge()
                    {
                        Caption = "Command",
                        BorderBrush = new SolidColorBrush(Colors.Green),
                        currentBadge = badge
                    };
                case Badges.OnlineSearch:
                    return new SearchResultBadge()
                    {
                        Caption = "OnlineSearch",
                        BorderBrush = new SolidColorBrush(Colors.White),
                        currentBadge = badge
                    };
                default:
                    return new SearchResultBadge()
                    {
                        Caption = "",
                        BorderBrush = new SolidColorBrush(Colors.Transparent),
                        currentBadge = badge
                    };
            }
        }
    }
}
