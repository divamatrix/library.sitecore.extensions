using Paragon.Sitecore.Extensions.Misc;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Links;

namespace Paragon.Sitecore.Extensions.ItemExtensions
{
    public static class ItemExtensions
    {
        /// <summary>
        /// Returns an item for the given id.
        /// </summary>
        /// <param name="itemId">string</param>
        /// <returns></returns>
        private static Item GetItemById(string itemId)
        {
            Database db = Context.Database;
            return itemId != null ? db.GetItem(itemId) : null;
        }

        public static bool IsNotNull(this Item item)
        {
            return !item.IsNull();
        }

        public static bool IsNull(this Item item)
        {
            return (item == null);
        }

        public static string GetUrl(this CustomItemBase item)
        {
            if ((item != null) && (item.InnerItem != null))
            {
                return item.InnerItem.GetUrl();
            }
            return string.Empty;
        }

        public static string GetUrl(this Item item)
        {
            if (item != null)
            {
                UrlOptions urlOptions = Context.Site.GetDefaultUrlOptions();
                urlOptions.SiteResolving = Settings.Rendering.SiteResolving;
                return LinkManager.GetItemUrl(item, urlOptions);
            }
            return string.Empty;
        }


    }
}