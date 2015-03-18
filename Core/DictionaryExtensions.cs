using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace Paragon.Sitecore.Extensions.Core
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Transforms a dictionary of key/value pairs into a string in query string format
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static string ToParamString(this Dictionary<string, string> dictionary)
        {
            StringBuilder sb = new StringBuilder();

            // Add all the key value pairs!
            foreach (string key in dictionary.Keys)
                sb.Append(key + "=" + dictionary[key] + "&");

            // Trim that trailing ampersand
            return sb.ToString().TrimEnd('&');
        }

        public static string ToParamString(this NameValueCollection dictionary)
        {
            StringBuilder sb = new StringBuilder();

            // Add all the key value pairs!
            foreach (string key in dictionary.Keys)
                sb.Append(key + "=" + dictionary[key] + "&");

            // Trim that trailing ampersand
            return sb.ToString().TrimEnd('&');
        }
    }
}