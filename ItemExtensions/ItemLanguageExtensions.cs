using System.Linq;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Diagnostics;
using Sitecore.Globalization;

namespace Paragon.Sitecore.Extensions.ItemExtensions
{
    public static class ItemLanguageExtensions
    {
        public static bool DoesCurrentLanguageVersionExist(this Item item)
        {
            return DoesCurrentLanguageVersionExist(item, Context.Language);

        }

        public static bool DoesCurrentLanguageVersionExist(this Item item, Language language)
        {
            Debug.ArgumentNotNull(language,"language");
            return ItemManager.GetVersions(item, language).Any();
        }

        public static bool DoesCurrentLanguageVersionExist(this Item item, string languageName)
        {
            Debug.ArgumentNotNull(languageName, "languageName");
            return DoesCurrentLanguageVersionExist(item, LanguageManager.GetLanguage(languageName));
        }

        public static int LanguageVersionCount(this Item item, Language lang)
        {
            Item currentItem = item.Database.GetItem (item.ID, lang);
            return currentItem.Versions.Count > 0 ? currentItem.Versions.Count : 0;
        }

        public static bool LanguageVersionIsEmpty(this Item item)
        {
            if (item != null)
            {
                Item langItem = item.Database.GetItem(item.ID, item.Language);
                if (langItem != null)
                {
                    return (langItem.Versions.Count == 0);
                }
            }
            return true;
        }

        public static Item GetBestFitLanguageVersion(this Item item)
        {
            Language generalizedLanguage;
            if (item == null)
            {
                return null;
            }
            if (item.LanguageVersionIsEmpty() && Language.TryParse(item.Language.Name.Substring(0, 2), out generalizedLanguage))
            {
                return item.Database.GetItem(item.ID, generalizedLanguage);
            }
            return item;
        }

        public static Item GetBestFitLanguageVersion(this Item item, Language targetLanguage)
        {
            return item.Database.GetItem(item.ID, targetLanguage).GetBestFitLanguageVersion();
        }





    }
}
