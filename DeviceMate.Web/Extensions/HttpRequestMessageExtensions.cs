using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace DeviceMate.Web.Extensions
{
    /// <summary>
    /// Extends the HttpRequestMessage collection
    /// </summary>
    [Obsolete]
    public static class HttpRequestMessageExtensions
    {
        /// <summary>
        /// Returns a dictionary of QueryStrings that's easier to work with 
        /// than GetQueryNameValuePairs KevValuePairs collection.
        /// 
        /// If you need to pull a few single values use GetQueryString instead.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetQueryStringAsDictionary(this HttpRequestMessage request)
        {
            try
            {
                return request.GetQueryNameValuePairs()
                              .ToDictionary(kv => kv.Key.ToLower(), kv => kv.Value,
                                   StringComparer.OrdinalIgnoreCase);
            }
            catch
            {
                //TODO: TS Log exception.
                return new Dictionary<string, string>();
            }
        }
    }
}