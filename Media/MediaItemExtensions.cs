using System;
using Sitecore;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Resources.Media;
using Convert = System.Convert;

namespace Paragon.Sitecore.Extensions.Media
{
    public static class MediaItemExtensions
    {
        // Methods
        public static MediaUrlOptions GetDefaultMediaUrlOptions()
        {
            return new MediaUrlOptions { LowercaseUrls = true, AbsolutePath = false, IncludeExtension = true, UseItemPath = false };
        }

        public static MediaUrlOptions GetDefaultMediaUrlOptions(this MediaItem mediaItem)
        {
            MediaUrlOptions options = GetDefaultMediaUrlOptions();
            if (mediaItem != null)
            {
                options.RequestExtension = mediaItem.Extension;
            }
            return options;
        }

        public static string GetMediaItemUrl(this MediaItem mediaItem)
        {
            return mediaItem != null ? MediaManager.GetMediaUrl(mediaItem, mediaItem.GetDefaultMediaUrlOptions()) : string.Empty;
        }

        public static string GetMediaItemUrl(this MediaItem mediaItem, MediaUrlOptions mediaUrlOptions)
        {
            return mediaItem != null ? MediaManager.GetMediaUrl(mediaItem, mediaUrlOptions) : string.Empty;
        }

        public static bool IsMediaItem(this Item item)
        {
            return item.Paths.FullPath.ToLower().Contains("/sitecore/media library");
        }

        public static MediaItem GetImageMediaItem(this Item item, string fieldName)
        {

            ImageField fieldValue = item.Fields[fieldName];
            MediaItem mediaItem = null;

            if (fieldValue != null && fieldValue.MediaItem != null)
                mediaItem = fieldValue.MediaItem;

            return mediaItem;
        }

        public static string GetImageMediaUrl(this Item item, string fieldName)
        {
            MediaItem media = item.GetImageMediaItem(fieldName);
            return media != null ? StringUtil.EnsurePrefix('/', MediaManager.GetMediaUrl(media)) : String.Empty;
        }

        public static string GetMediaSize(this Item item, string fieldName)
        {
            var size = "";

            ImageField image = item.Fields[fieldName];
            if (image != null)
            {
                if (image.MediaItem != null)
                {
                    if (!String.IsNullOrEmpty(image.MediaItem["Size"]))
                    {
                        var sizeInMb = Convert.ToDecimal(size) / 1000000;
                        size = String.Format("{0:#.##}", sizeInMb) + "mb";
                    }
                }
            }
            return size;
        }
    }
}
