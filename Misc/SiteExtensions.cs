using System;
using System.Globalization;
using System.Linq;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Links;
using Sitecore.Sites;
using Sitecore.Web;

namespace Paragon.Sitecore.Extensions.Misc
{
    public static class SiteExtensions
    {

        public static bool IsInContextSite(this Item context)
        {
            if (context == null)
            {
                return false;
            }
            Assert.IsNotNull(Context.Site, "Sitecore.Context.Site required by the Item.IsInContextSite extension is null");
            Assert.IsNotNullOrEmpty(Context.Site.RootPath, "Sitecore.Context.Site.RootPath required by the Item.IsInSite extension is null or empty");
            Assert.IsNotNull(Context.Database, "Sitecore.Context.Database required by the Item.IsInSite extension is null");
            Item rootItem = Context.Site.Database.Items[Context.Site.RootPath];
            Assert.IsNotNull(rootItem, String.Format(CultureInfo.CurrentCulture, "Unable to retrieve the item at path {0} using the database {1}", Context.Site.RootPath, Context.Database.Name));
            return rootItem.Axes.IsAncestorOf(context);
        }


        public static Item GetHomeItem(this SiteContext me, Database database)
        {
            Assert.ArgumentNotNull(database, "database");
            return database.GetItem(me.StartPath);
        }

        public static Item GetHomeItem(this SiteContext me)
        {
            return GetHomeItem(me, Context.Database);
        }

        public static SiteContext ResolveSite(this Language lang)
        {
            SiteContext siteContext;

            string querystring = WebUtil.ExtractUrlParm("sc_site", WebUtil.GetRawUrl());
            if (!String.IsNullOrEmpty(querystring))
            {
                siteContext = SiteContextFactory.GetSiteContext(querystring);
                if (siteContext != null)
                {
                    return siteContext;
                }
            }

            querystring = WebUtil.ExtractUrlParm("site", WebUtil.GetRawUrl());
            if (!String.IsNullOrEmpty(querystring))
            {
                siteContext = SiteContextFactory.GetSiteContext(querystring);
                if (siteContext != null)
                {
                    return siteContext;
                }
            }

            siteContext = SiteContextFactory.GetSiteContext(WebUtil.GetHostName(),
                WebUtil.GetRawUrl().Replace("/" + lang + "/", "/"));
            if (siteContext != null)
            {
                return siteContext;
            }

            return Context.Site;
        }

        public static UrlOptions GetDefaultUrlOptions(this SiteContext site)
        {
            UrlOptions defaultOptions = new UrlOptions
            {
                AddAspxExtension = false,
                ShortenUrls = true,
                LowercaseUrls = true
            };

            UrlOptions options = defaultOptions;
            LanguageEmbedding? languageEmbedding = site.GetLanguageEmbedding();
            if (languageEmbedding.HasValue)
            {
                options.LanguageEmbedding = languageEmbedding.Value;
            }
            return options;
        }

        public static int GetSitemapLevel(this Item item, Item homeItem)
        {
            if (item == null || homeItem == null) { return -1; }
            Item parent = item.Parent;
            if (parent == null) { return -1; }
            if (parent.ID == homeItem.ID)
            {
                return 0;
            }
            else
            {
                return 1 + parent.GetSitemapLevel(homeItem);
            }
        }

        public static SiteInfo GetHomeSite(this Item item)
        {
            return Factory.GetSiteInfoList().FirstOrDefault(x => item.Paths.FullPath.StartsWith(x.RootPath));
        }

        public static Item GetHomeItem(this Item item)
        {
            return Context.ContentDatabase.GetItem(item.GetHomeSite().StartItem);
        }
    }
}
