using System;
using System.Collections.Specialized;
using System.Linq;

namespace Paragon.Sitecore.Extensions.Core
{
    public static class NameValueCollectionExtensions
    {
        public static string CheckParameterKeySet(this NameValueCollection parameters, params string[] keys)
        {
            return (from key in keys where !String.IsNullOrWhiteSpace(parameters[key]) select parameters[key]).FirstOrDefault();
        }
    }
}