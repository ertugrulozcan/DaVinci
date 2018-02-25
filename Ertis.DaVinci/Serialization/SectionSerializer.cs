using Ertis.DaVinci.HtmlModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.DaVinci.Serialization
{
    public class SectionSerializer : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(Section));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);
            switch ((SectionType)jsonObject["Type"].Value<Int64>())
            {
                case SectionType.BannerSection:
                {
                    var banner = jsonObject.ToObject<Banner>(serializer);
                    return banner;
                }
                case SectionType.MetroSection:
                    {
                        var metroSection = jsonObject.ToObject<MetroSection>(serializer);
                        return metroSection;
                    }
                case SectionType.BasicSection:
                    {
                        var basicSection = jsonObject.ToObject<BasicSection>(serializer);
                        return basicSection;
                    }
                case SectionType.CardsSection:
                    {
                        var cardsSection = jsonObject.ToObject<CardsSection>(serializer); ;
                        return cardsSection;
                    }
                case SectionType.ImageSection:
                    {
                        var imageSection = jsonObject.ToObject<ImageSection>(serializer);
                        return imageSection;
                    }
                case SectionType.ParagraphSection:
                    {
                        var paragraphSection = jsonObject.ToObject<ParagraphSection>(serializer); ;
                        return paragraphSection;
                    }
                case SectionType.BlogSection:
                    {
                        var blogSection = jsonObject.ToObject<BlogSection>(serializer); ;
                        return blogSection;
                    }
                case SectionType.ContactSection:
                    {
                        var contactSection = jsonObject.ToObject<ContactSection>(serializer); ;
                        return contactSection;
                    }
                case SectionType.GoogleMapsSection:
                    {
                        var googleMapsSection = jsonObject.ToObject<GoogleMapsSection>(serializer); ;
                        return googleMapsSection;
                    }
                case SectionType.Gallery:
                    {
                        var gallerySection = jsonObject.ToObject<GallerySection>(serializer); ;
                        return gallerySection;
                    }
                default:
                    break;
            }

            return null;
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
