using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paragon.Sitecore.Extensions.Core
{
    public static class HtmlExtensions
    {
        public static string ToUnorderedList(string stringList, string cssClass, char separator)
        {
            IEnumerable<string> listItems;
            try
            {
                listItems = new List<string>(stringList.Split(separator).ToList());
            }
            catch (ArgumentNullException)
            {
                return null;
            }
            return ToUnorderedList(listItems, cssClass);
        }

        public static string ToUnorderedList(this IEnumerable<string> list, string cssClass = "")
        {
            var sb = new StringBuilder();

            sb.AppendLine(string.Format("<ul class=\"{0}\">", cssClass));

            foreach (string listItem in list)
            {
                sb.AppendLine(
                    string.Format("<li>{0}</li>", listItem)
                );
            }

            sb.AppendLine("</ul>");

            return sb.ToString();
        }
    }
}
