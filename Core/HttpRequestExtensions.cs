using System.Web;

namespace Paragon.Sitecore.Extensions.Core
{
    public static class HttpRequestExtensions
    {
   
        /// <summary>
        /// Gets a secure URL for a requested resource
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetSecureUrl(this HttpRequest request)
        {
            return "https://" + request.Url.Host + request.Url.PathAndQuery;
        }   
    }
}
