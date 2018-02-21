using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TwitterReader
{
    public class TweetModel
    {
        public long TweetId { get; set; }
        public string TweetUrl { get; set; }
        public string TweetFullText { get; set; }
        public bool IsRetweet { get; set; }
        public string TweetEmbedUrl { get; set; }
        public string TweetImageUrl { get; set; }
        public bool TweetImageUrlNotEmpty => !string.IsNullOrWhiteSpace(TweetImageUrl);
        public string TweetDateTime { get; set; }
    }
}
