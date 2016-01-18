using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FableProject.Functions
{
    public static class UriExtensions
    {
        public static Uri CreateUriWithQuery(Uri uri, NameValueCollection values)
        {
            var queryStr = new StringBuilder();
            // presumes that if there's a Query set, it starts with a ?
            var str = string.IsNullOrWhiteSpace(uri.Query) ?
                      "" : uri.Query.Substring(1) + "&";

            foreach (var value in values)
            {
                queryStr.Append(str + value.Key + "=" + value.Value);
                str = "&";
            }
            // query string will be encoded by building a new Uri instance
            // clobbers the existing Query if it exists
            return new UriBuilder(uri)
            {
                Query = queryStr.ToString()
            }.Uri;
        }
        public static Uri CreateUriWithQuery(Uri uri, NameValueCollection values, NameValueCollection values2)
        {
            var queryStr = new StringBuilder();
            // presumes that if there's a Query set, it starts with a ?
            var str = string.IsNullOrWhiteSpace(uri.Query) ?
                      "" : uri.Query.Substring(1) + "&";

            foreach (var value in values)
            {
                queryStr.Append(str + value.Key + "=" + value.Value);
                str = "&";
            }
            foreach (var value2 in values2)
            {
                queryStr.Append(str + value2.Key + "=" + value2.Value);
                str = "&";
            }
            // query string will be encoded by building a new Uri instance
            // clobbers the existing Query if it exists
            return new UriBuilder(uri)
            {
                Query = queryStr.ToString()
            }.Uri;
        }

        public static Uri CreateUriWithQuery(Uri uri, NameValueCollection values, NameValueCollection values2, NameValueCollection values3)
        {
            var queryStr = new StringBuilder();
            // presumes that if there's a Query set, it starts with a ?
            var str = string.IsNullOrWhiteSpace(uri.Query) ?
                      "" : uri.Query.Substring(1) + "&";

            foreach (var value in values)
            {
                queryStr.Append(str + value.Key + "=" + value.Value);
                str = "&";
            }
            foreach (var value2 in values2)
            {
                queryStr.Append(str + value2.Key + "=" + value2.Value);
                str = "&";
            }
            foreach (var value3 in values3)
            {
                queryStr.Append(str + value3.Key + "=" + value3.Value);
                str = "&";
            }
            // query string will be encoded by building a new Uri instance
            // clobbers the existing Query if it exists
            return new UriBuilder(uri)
            {
                Query = queryStr.ToString()
            }.Uri;
        }

    }
    public class NameValueCollection : Dictionary<string, string>
    {
    }

}
