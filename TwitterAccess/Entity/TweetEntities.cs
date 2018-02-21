using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterAccess
{
    public class TweetEntities
    {
        [JsonProperty("urls")]
        public List<UrlEntity> UrlList { get; set; }

        [JsonProperty("media")]
        public List<MediaEntity> MediaList { get; set; }
    }
}
