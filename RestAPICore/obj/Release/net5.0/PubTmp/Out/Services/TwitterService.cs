using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace RestAPICore.Services
{
    public class TwitterService
    {
        public string OAuthConsumerSecret { get; set; }
        public string OAuthConsumerKey { get; set; }
        //get token
        public async Task<string> GetAccessToken()
        {
            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.twitter.com/oauth2/token ");
            var customerInfo = Convert.ToBase64String(new UTF8Encoding()
                                        .GetBytes(OAuthConsumerKey + ":" + OAuthConsumerSecret));
            request.Headers.Add("Authorization", "Basic " + customerInfo);
            request.Content = new StringContent("grant_type=client_credentials",
                                                    Encoding.UTF8, "application/x-www-form-urlencoded");
            HttpResponseMessage response = await httpClient.SendAsync(request).ConfigureAwait(false);
            string json = await response.Content.ReadAsStringAsync();
            dynamic item = Newtonsoft.Json.JsonConvert.DeserializeObject<object>(json);
            return item["access_token"];
        }
        //Tweets by Location
        public List<Models.TwitterModel.Status> getTweetsByGeo(string lat, string lng, int radius, int count, string accessToken = null)
        {
            if (accessToken == null)
            {
                accessToken = GetAccessToken().GetAwaiter().GetResult();
            }
            var requestGeoTimeline = new HttpRequestMessage(HttpMethod.Get, string.Format(@"
https://api.twitter.com/1.1/search/tweets.json?count={3}&geocode={0},{1},{2}mi&exclude_replies=1&tweet_mode=extended"
           , lat, lng, radius, count));
            requestGeoTimeline.Headers.Add("Authorization", "Bearer " + accessToken);
            var http = new HttpClient();
            HttpResponseMessage responseUserTimeLine = http.SendAsync(requestGeoTimeline).GetAwaiter().GetResult();
            string json = responseUserTimeLine.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            return JsonConvert.DeserializeObject<Models.TwitterModel.Rootobject>(json).statuses.ToList<Models.TwitterModel.Status>();
        }
        //Tweets by User
        public List<Models.TwitterModel.Status> getTweetsByUser(string userName, string lookBack, int count, string accessToken = null)
        {
            if (accessToken == null)
            {
                accessToken = GetAccessToken().GetAwaiter().GetResult();
            }
            var requestUserTimeline = new HttpRequestMessage(HttpMethod.Get, string.Format(@"
            https://api.twitter.com/1.1/statuses/user_timeline.json?count={0}&screen_name={1}&until={2}&exclude_replies=1&tweet_mode=extended"
            , count, userName, lookBack));
            requestUserTimeline.Headers.Add("Authorization", "Bearer " + accessToken);
            var httpClient = new HttpClient();
            HttpResponseMessage responseUserTimeLine = httpClient.SendAsync(requestUserTimeline).GetAwaiter().GetResult();
            string json = responseUserTimeLine.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            return JsonConvert.DeserializeObject<List<Models.TwitterModel.Status>>(json);
        }
        //Tweets by Term
        public List<Models.TwitterModel.Status> getTweetsByTerm(string term, int count, string accessToken = null)
        {
            if (accessToken == null)
            {
                accessToken = GetAccessToken().GetAwaiter().GetResult();
            }
            var requestUserTimeline = new HttpRequestMessage(HttpMethod.Get, string.Format(@"
            https://api.twitter.com/1.1/search/tweets.json?count={0}&q={1}&tweet_mode=extended"
            , count, term));
            requestUserTimeline.Headers.Add("Authorization", "Bearer " + accessToken);
            var httpClient = new HttpClient();
            HttpResponseMessage responseUserTimeLine = httpClient.SendAsync(requestUserTimeline).GetAwaiter().GetResult();
            string json = responseUserTimeLine.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            List<Models.TwitterModel.Status> response = JsonConvert.DeserializeObject<Models.TwitterModel.Rootobject>(json).statuses.ToList<Models.TwitterModel.Status>();
            List<Models.TwitterModel.Status> og = response.FindAll(p => p.in_reply_to_status_id == null);
            response.FindAll(p => p.in_reply_to_status_id == null).ForEach(x => {
                x.comment_count = response.FindAll(t => t.in_reply_to_status_id_str != null && t.in_reply_to_status_id_str.Equals(x.id_str)).Count();
            });

            return response;
        }

        //Tweets by Term Since Search
        public List<Models.TwitterModel.Status> getTweetsByTermSinceID(string term, int count, string  since_id, string accessToken = null)
        {
            if (accessToken == null)
            {
                accessToken = GetAccessToken().GetAwaiter().GetResult();
            }
            var requestUserTimeline = new HttpRequestMessage(HttpMethod.Get, string.Format(@"
            https://api.twitter.com/1.1/search/tweets.json?count={0}&q={1}&since_id={2}&exclude_replies=1&tweet_mode=extended"
            , count, term, since_id));
            requestUserTimeline.Headers.Add("Authorization", "Bearer " + accessToken);
            var httpClient = new HttpClient();
            HttpResponseMessage responseUserTimeLine = httpClient.SendAsync(requestUserTimeline).GetAwaiter().GetResult();
            string json = responseUserTimeLine.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            return JsonConvert.DeserializeObject<Models.TwitterModel.Rootobject>(json).statuses.ToList<Models.TwitterModel.Status>();
        }

        //Tweets by Term Since Search
        public List<Models.TwitterModel.Status> getTweetsByTermAfterID(string term, int count, string after_id, string accessToken = null)
        {
            if (accessToken == null)
            {
                accessToken = GetAccessToken().GetAwaiter().GetResult();
            }
            var requestUserTimeline = new HttpRequestMessage(HttpMethod.Get, string.Format(@"
            https://api.twitter.com/1.1/search/tweets.json?count={0}&q={1}&max_id={2}&exclude_replies=1&tweet_mode=extended"
            , count, term, after_id));
            requestUserTimeline.Headers.Add("Authorization", "Bearer " + accessToken);
            var httpClient = new HttpClient();
            HttpResponseMessage responseUserTimeLine = httpClient.SendAsync(requestUserTimeline).GetAwaiter().GetResult();
            string json = responseUserTimeLine.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            return JsonConvert.DeserializeObject<Models.TwitterModel.Rootobject>(json).statuses.ToList<Models.TwitterModel.Status>();
        }
    }
}
