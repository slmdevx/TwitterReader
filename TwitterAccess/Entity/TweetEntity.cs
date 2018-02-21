using System;
using Newtonsoft.Json;

namespace TwitterAccess
{
    public class TweetEntity
    {                
        [JsonProperty("id")]
        public long Id { get; set; }        
        
        [JsonProperty("full_text")]
        public string FullText { get; set; }

        [JsonProperty("user")]
        public UserEntity CreatedBy { get; set; }

        [JsonProperty("entities")]
        public TweetEntities LegacyEntities { get; set; }

        [JsonProperty("extended_entities")]
        public TweetEntities Entities { get; set; }

        [JsonProperty("created_at")]
        [JsonConverter(typeof(JsonDateTimeConverter))]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("retweeted_status")]
        public TweetEntity RetweetedTweet { get; set; }
    }
}
