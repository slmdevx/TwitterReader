using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterAccess
{
    public class TwitterConstants
    {
        public const string AuthUserUrl = "https://api.twitter.com/1.1/account/verify_credentials.json";
        public const string UserTweetsUrl = "https://api.twitter.com/1.1/statuses/user_timeline.json";
        public const string FriendIdsUrl = "https://api.twitter.com/1.1/friends/ids.json";
        public const string UsersDataUrl = "https://api.twitter.com/1.1/users/lookup.json";
        public const string TonUrl = "https://ton.twitter.com";
        public const int MaxFriendsToRetrive = 120;
    }
}
