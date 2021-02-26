using Microsoft.AspNetCore.Mvc;

namespace RestAPICore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Twitter2Controller : ControllerBase
    {
        private Services.Twitter2Service twitterService = new Services.Twitter2Service
        {
            //Your Twitter App details here
            OAuthConsumerKey = Services.CredentialService.twitterKey,
            OAuthConsumerSecret = Services.CredentialService.twitterSecret
        };

        /// <summary>
        /// Search Twitter by Keyword
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="tweetCount"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("searchRecent")]
        public Models.Twitter2Model.Rootobject getSearchRecent(string keyword, int tweetCount)
        {
            if (tweetCount > 100)
            {
                tweetCount = 100;
            }
            return twitterService.getSearchAll(keyword, tweetCount);
        }

        /// <summary>
        /// Search Twitter by Keyword For Display
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="tweetCount"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("search")]
        public Models.Twitter2Model.RootView getSearchRecentForDisplay(string keyword, int tweetCount)
        {
            if (tweetCount > 100)
            {
                tweetCount = 100;
            }
            return twitterService.getSearchAllForDisplay(keyword, tweetCount);
        }
    }
}