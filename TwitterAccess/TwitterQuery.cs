using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TwitterAccess
{
    public class TwitterQuery
    {
        private TwitterQuery()
        {
            QueryParameterList = new List<KeyValuePair<string, string>>();
        }

        public static TwitterQuery Create(HttpMethod httpMethod, string url)
        {
            return new TwitterQuery
            {
                QueryUrl = url
            };
        }

        public string QueryUrl { get; private set; }
        public List<KeyValuePair<string, string>> QueryParameterList { get; }
        
        public void AddParameter(string key, object value)
        {
            AddParameter(key, value?.ToString());
        }

        public void AddParameter(string key, string value)
        {
            if (QueryParameterList.Count == 0)
            {
                QueryUrl += $"?{key}={value}";
            }
            else
            {
                QueryUrl += $"&{key}={value}";
            }

            // Build key / value paramater list for signing
            QueryParameterList.Add(new KeyValuePair<string, string>(key, value));
        }
    }
}
