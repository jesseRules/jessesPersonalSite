using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPICore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TwitterController : ControllerBase
    {
        Services.TwitterService twitterService = new Services.TwitterService
        {
            //Your Twitter App details here
            OAuthConsumerKey = Services.CredentialService.twitterKey,
            OAuthConsumerSecret = Services.CredentialService.twitterSecret
        };
        /// <summary>
        /// Get Tweets bt UserName limit 100
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="tweetCount"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("byuser")]
        public List<Models.TwitterModel.Status> getTweetsByUser(string userName, int tweetCount)
        {
            if (tweetCount > 100)
            {
                tweetCount = 100;
            }
            string lookBack = DateTime.Now.ToString("yyyy-MM-dd");
            return twitterService.getTweetsByUser(userName, lookBack, tweetCount);
        }
        /// <summary>
        /// Get Tweets by Geo Location 
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <param name="radiusMiles"></param>
        /// <param name="tweetCount"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("bygeo")]
        public List<Models.TwitterModel.Status> getTweetsByGeoLocation(string lat, string lng, int radiusMiles, int tweetCount)
        {
            if (tweetCount > 100)
            {
                tweetCount = 100;
            }
            return twitterService.getTweetsByGeo(lat, lng, radiusMiles, tweetCount);
        }
        /// <summary>
        /// Search Twitter by Keyword
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="tweetCount"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("byterm")]
        public List<Models.TwitterModel.Status> getTweetsByKeyword(string keyword, int tweetCount)
        {
            if (tweetCount > 100)
            {
                tweetCount = 100;
            }
            return twitterService.getTweetsByTerm(keyword, tweetCount);
        }

        /// <summary>
        /// Search Twitter by Keyword
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="tweetCount"></param>
        /// <param name="afterId"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("byterm/afterId")]
        public List<Models.TwitterModel.Status> getTweetsByKeywordAfterId(string keyword, int tweetCount, string afterId)
        {
            if (tweetCount > 100)
            {
                tweetCount = 100;
            }
            return twitterService.getTweetsByTermAfterID(keyword, tweetCount, afterId);
        }
    }
}
