using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TwitterAccess
{
    public class UserEntity
    {
        [JsonProperty("id")]
        public long Id { get; set; }
      
        [JsonProperty("screen_name")]
        public string ScreenName { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
           
        [JsonProperty("profile_image_url")]
        public string ProfileImageUrl { get; set; }
               
        public override string ToString()
        {
            return $"{Name} | {ScreenName} | {Id}";
        }
    }
}
