using System;
using System.Collections.Generic;

namespace Ertis.WebService.Localization
{
    public class LocalizationDictionary
    {
        public string Culture { get; private set; }

        public string Language { get; private set; }

        public string Region { get; private set; }

        public Dictionary<string, string> KeyValueDictionary { get; private set; }

        public LocalizationDictionary(string culture, string language, string region)
        {
            this.Culture = culture;
            this.Language = language;
            this.Region = region;

            this.KeyValueDictionary = new Dictionary<string, string>();
        }

        public string Get(string key)
        {
            if (string.IsNullOrEmpty(key))
                return "Key:NULL";
            
            if (this.KeyValueDictionary.ContainsKey(key))
                return this.KeyValueDictionary[key];
            else
                return string.Format("Key:{0}", key);
        }

        public void Add(string key, string value)
        {
            if (this.KeyValueDictionary.ContainsKey(key))
                return;

            this.KeyValueDictionary.Add(key, value);
        }
    }
}
