using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.Infrastructure.Application
{
    public class AppTranslation
    {
        public string Language { get; set; }
        public string ShortKey { get; set; }
        public string StrKey { get; set; }
        public string StrValue { get; set; }
        public DateTime DateAdded { get; set; }

        public AppTranslation() { }

        public AppTranslation(string language, string shortKey, string strKey, string strValue, DateTime dateAdded)
        {
            this.Language = language;
            this.ShortKey = shortKey;
            this.StrKey = strKey;
            this.StrValue = strValue;
            this.DateAdded = dateAdded;
        }
    }
}
