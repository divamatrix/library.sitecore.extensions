

using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.Links;
using Sitecore.Sites;
using Sitecore.Web;

namespace Paragon.Sitecore.Extensions.Misc
{
    public static class LanguageExtensions
    {


        // Methods
        public static Item GetLanguageItem(this Language language)
        {
            return GetLanguageItem(language, Context.Database);
        }

        public static Item GetLanguageItem(this Language language, Database database)
        {
            return database.GetItem("/sitecore/system/languages/" + language.Name, language);
        }

        public static string GetNaturalDisplayName(this Language language)
        {
            return GetNaturalDisplayName(language, Context.Database);
        }

        public static string GetNaturalDisplayName(this Language language, Database database)
        {
            return GetLanguageItem(language, database).DisplayName;
        }


        public static LanguageEmbedding? GetLanguageEmbedding(this SiteContext siteContext,
            string siteInfoProperty = "languageEmbedding")
        {
            if (siteContext != null)
            {
                LanguageEmbedding languageEmbedding;
                string value = siteContext.Properties[siteInfoProperty];
                if (!string.IsNullOrWhiteSpace(value) && Enum.TryParse(value, true, out languageEmbedding))
                {
                    return languageEmbedding;
                }
            }
            return null;
        }

        public static LanguageEmbedding? GetLanguageEmbedding(this SiteInfo siteInfo,
            string siteInfoProperty = "languageEmbedding")
        {
            if (siteInfo != null)
            {
                LanguageEmbedding languageEmbedding;
                string value = siteInfo.Properties[siteInfoProperty];
                if (!string.IsNullOrWhiteSpace(value) && Enum.TryParse(value, true, out languageEmbedding))
                {
                    return languageEmbedding;
                }
            }
            return null;
        }

        public static ICollection<string> GetSupportedLanguages(this SiteContext siteContext,
            string siteInfoProperty = "supportedLanguages")
        {
            if (siteContext == null)
            {
                return new string[0];
            }
            return siteContext.SiteInfo.GetSupportedLanguages(siteInfoProperty);
        }

        public static ICollection<string> GetSupportedLanguages(this SiteInfo siteInfo,
            string siteInfoProperty = "supportedLanguages")
        {
            if (siteInfo == null)
            {
                return new string[0];
            }
            string supportedLanguagesAttributeValue = siteInfo.Properties[siteInfoProperty];
            if (string.IsNullOrWhiteSpace(supportedLanguagesAttributeValue))
            {
                return new string[0];
            }
            List<string> languageCodes = new List<string>();
            if (!string.IsNullOrEmpty(supportedLanguagesAttributeValue))
            {
                string[] separated = supportedLanguagesAttributeValue.Split(new[] { ',', ' ', '|' },
                    StringSplitOptions.RemoveEmptyEntries);
                languageCodes.AddRange(from code in separated
                                       where !string.IsNullOrEmpty(code)
                                       select code);
            }
            string primaryLanguageCode = siteInfo.Language;
            if (!languageCodes.Contains(primaryLanguageCode))
            {
                languageCodes.Insert(0, primaryLanguageCode);
            }
            return languageCodes;
        }
    }
}
