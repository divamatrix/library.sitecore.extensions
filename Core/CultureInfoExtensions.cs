using System.Globalization;

namespace Paragon.Sitecore.Extensions.Core
{
    public static class CultureInfoExtensions
    {
        public static string GetBaseName(this CultureInfo cultureInfo)
        {
            // Contains a dash? Get the two letter version
            return cultureInfo.Name.Contains("-") ? new CultureInfo(cultureInfo.TwoLetterISOLanguageName).EnglishName : cultureInfo.EnglishName;
        }
    }
}