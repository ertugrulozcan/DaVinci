using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;

namespace Ertis.WebService.Localization
{
    public class LocalizationManager
    {
        private readonly string LocalizationDataSourcePath = @"/Localization/Data";

        private static LocalizationManager self;

        public static LocalizationManager Current
        {
            get
            {
                if (self == null)
                    self = new LocalizationManager();

                return self;
            }
        }

        private List<LocalizationDictionary> DictionaryList { get; set; }

        private LocalizationManager()
        {
            this.DictionaryList = new List<LocalizationDictionary>();
        }

        public void LoadFromResource(IHostingEnvironment hostingEnvironment, string culture)
        {
            try
            {
                string contentRootPath = hostingEnvironment.ContentRootPath;
                var jsonString = System.IO.File.ReadAllText(contentRootPath + LocalizationDataSourcePath + "/" + culture + ".json");

                var loc = Newtonsoft.Json.JsonConvert.DeserializeObject<LocalizationDictionary>(jsonString);
                this.DictionaryList.Add(loc);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("LocalizationManager Error!");
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        public LocalizationDictionary GetDictionary(string culture)
        {
            if (this.DictionaryList.Any(x => x.Culture.Contains(culture)))
                return this.DictionaryList.First(x => x.Culture.Contains(culture));

            return null;
        }

        public string GetDictionaryAsJson(string culture)
        {
            var dict = this.GetDictionary(culture);
            if (dict != null)
                return Newtonsoft.Json.JsonConvert.SerializeObject(dict, Newtonsoft.Json.Formatting.Indented);

            return null;
        }
    }
}
