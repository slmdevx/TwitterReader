using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TwitterAccess
{
    public class UrlEntity
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        public override string ToString()
        {
            return Url;
        }

    }
}
