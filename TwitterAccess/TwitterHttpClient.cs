using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TwitterAccess
{
    public class TwitterHttpClient
    {
        private TwitterCredentials _twitterCreds;

        public TwitterHttpClient(TwitterCredentials twitterCreds)
        {
            _twitterCreds = twitterCreds;
        }

        public UserEntity GetAuthenticatedUser()
        {
            TwitterQuery twitterQuery = TwitterQuery.Create(HttpMethod.Get, TwitterConstants.AuthUserUrl);
            twitterQuery.AddParameter("skip_status", "true");
            twitterQuery.AddParameter("include_entities", "true");
            string result = ExecuteQuery(twitterQuery);
            UserEntity user = JsonHelper.DeserializeToClass<UserEntity>(result);
            return user;
        }

        public List<UserEntity> GetFriends(long userId)
        {
            List<UserEntity> friendList = null;
            long[] friendIds = GetFriendIds(userId);
            if (friendIds.Length > 0)
            {
                friendList = GetFriendsFromIds(friendIds);
            }
            return friendList ?? new List<UserEntity>();
        }

        public List<TweetEntity> GetUserTweetList(long userId, int count, bool includeRetweet = false)
        {
            var twitterQuery = TwitterQuery.Create(HttpMethod.Get, TwitterConstants.UserTweetsUrl);
            twitterQuery.AddParameter("user_id", userId);
            twitterQuery.AddParameter("include_rts", includeRetweet);
            twitterQuery.AddParameter("exclude_replies", false);
            twitterQuery.AddParameter("contributor_details", false);
            twitterQuery.AddParameter("count", count);
            twitterQuery.AddParameter("trim_user", false);
            twitterQuery.AddParameter("include_entities", true);
            twitterQuery.AddParameter("tweet_mode", "extended");

            string result = ExecuteQuery(twitterQuery);
            var tweetList = JsonHelper.DeserializeToClass<List<TweetEntity>>(result);
            return tweetList ?? new List<TweetEntity>();
        }

        private long[] GetFriendIds(long userId)
        {
            var twitterQuery = TwitterQuery.Create(HttpMethod.Get, TwitterConstants.FriendIdsUrl);
            twitterQuery.AddParameter("user_id", userId);
            twitterQuery.AddParameter("count", TwitterConstants.MaxFriendsToRetrive);
            string result = ExecuteQuery(twitterQuery);
            var resultIds = JsonHelper.DeserializeToClass<ResultIds>(result);
            return resultIds.Ids ?? new long[0];
        }

        private List<UserEntity> GetFriendsFromIds(long[] friendIds)
        {
            var twitterQuery = TwitterQuery.Create(HttpMethod.Post, TwitterConstants.UsersDataUrl);
            string userIdsParam = GenerateIdsParameter(friendIds);
            twitterQuery.AddParameter("user_id", userIdsParam);
            string result = ExecuteQuery(twitterQuery);
            List<UserEntity> userList = JsonHelper.DeserializeToClass<List<UserEntity>>(result);
            return userList ?? new List<UserEntity>();
        }

        private string GenerateIdsParameter(long[] ids)
        {
            var result = new StringBuilder();
            for (int i = 0; i < ids.Length - 1; ++i)
            {
                result.Append(string.Format("{0}%2C", ids[i]));
            }
            result.Append(ids[ids.Length - 1]);
            return result.ToString();
        }

        private string ExecuteQuery(TwitterQuery twitterQuery)
        {
            string queryResult = null;
            HttpResponseMessage httpResponseMessage = null;

            try
            {
                httpResponseMessage = GetHttpResponseAsync(twitterQuery).Result;                
                var stream = httpResponseMessage.Content.ReadAsStreamAsync().Result;                
                if (httpResponseMessage.IsSuccessStatusCode && stream != null)
                {
                    var responseReader = new StreamReader(stream);
                    queryResult = responseReader.ReadLine();
                }
            }
            catch (Exception)
            {
                if (httpResponseMessage != null)
                {
                    httpResponseMessage.Dispose();
                }
            }

            return queryResult;
        }        

        public async Task<HttpResponseMessage> GetHttpResponseAsync(TwitterQuery twitterQuery)
        {
            string authorizationHeader = GenerateAuthorizationHeader(twitterQuery);
            using (var httpClient = new HttpClient(new TwitterHttpClientHandler(authorizationHeader)))
            {
                return await httpClient
                                .SendAsync(new HttpRequestMessage(HttpMethod.Get, twitterQuery.QueryUrl))
                                .ConfigureAwait(false);
            }
        }

        private string GenerateAuthorizationHeader(TwitterQuery twitterQuery)
        {
            var signatureParameters = new List<KeyValuePair<string, string>>();
            foreach (var queryParameter in twitterQuery.QueryParameterList)
            {
                signatureParameters.Add(new KeyValuePair<string, string>(queryParameter.Key, queryParameter.Value));
            }

            var uri = new Uri(twitterQuery.QueryUrl);
            string oauthNonce = new Random().Next(123400, 9999999).ToString(CultureInfo.InvariantCulture);
            var dateTime = DateTime.UtcNow;
            TimeSpan ts = dateTime - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            string oauthTimestamp = Convert.ToInt64(ts.TotalSeconds).ToString(CultureInfo.InvariantCulture);

            signatureParameters.AddRange(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("oauth_consumer_key", _twitterCreds.ConsumerKey),
                new KeyValuePair<string, string>("oauth_nonce", oauthNonce),
                new KeyValuePair<string, string>("oauth_signature_method", "HMAC-SHA1"),
                new KeyValuePair<string, string>("oauth_timestamp", oauthTimestamp),
                new KeyValuePair<string, string>("oauth_token", _twitterCreds.UserAccessToken),
                new KeyValuePair<string, string>("oauth_version", "1.0"),
            });

            StringBuilder header = new StringBuilder("OAuth ");

            // Generate OAuthRequest Parameters
            StringBuilder urlParametersFormatted = new StringBuilder();
            foreach (KeyValuePair<string, string> param in (from p in signatureParameters orderby p.Key select p))
            {
                // 1) Generate header
                if (param.Key.StartsWith("oauth_"))
                {
                    if (header.Length > 6)
                    {
                        header.Append(",");
                    }
                    header.Append(string.Format("{0}=\"{1}\"", param.Key, param.Value));
                }

                // 2) Generate data for signature to be used later
                if (urlParametersFormatted.Length > 0)
                {
                    urlParametersFormatted.Append("&");
                }
                urlParametersFormatted.Append(string.Format("{0}={1}", param.Key, param.Value));
            }

            // Generate OAuthRequest
            string url = (uri.Query == string.Empty) ? uri.AbsoluteUri : uri.AbsoluteUri.Replace(uri.Query, string.Empty);
            string oAuthRequest = string.Format("{0}&{1}&{2}",
                HttpMethod.Get,
                StringHelper.UrlEncode(url),
                StringHelper.UrlEncode(urlParametersFormatted.ToString()));

            // Generate OAuthSecretKey
            string oAuthSecretkey = StringHelper.UrlEncode(_twitterCreds.ConsumerSecret) + "&" +
                                    StringHelper.UrlEncode(_twitterCreds.UserAccessSecret);

            // Create signature
            HMACSHA1Generator hmacsha1Generator = new HMACSHA1Generator();
            string signature = StringHelper.UrlEncode(Convert.ToBase64String(hmacsha1Generator.ComputeHash(oAuthRequest, oAuthSecretkey, Encoding.UTF8)));

            // Append signature to Header
            header.Append($",oauth_signature=\"{signature}\"");

            return header.ToString();
        }
    }
}
