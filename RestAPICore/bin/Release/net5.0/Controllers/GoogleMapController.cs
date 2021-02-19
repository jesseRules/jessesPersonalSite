using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace RestAPICore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoogleController : ControllerBase
    {
        /// <summary>
        /// Returns a list of Map Markers
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("markers")]
        public List<Models.GoogleModel> getMarkers()
        {
            List<Models.GoogleModel> markerList = new List<Models.GoogleModel>();
            Models.GoogleModel marker = new Models.GoogleModel();
            marker.ceo = "Paul Brown";
            marker.lat = "33.930450";
            marker.lng = "-84.349250";
            marker.icon = "arbys-logo.png";
            marker.title = "They have the meats!";
            markerList.Add(marker);
            marker = new Models.GoogleModel();
            marker.ceo = "Sundar Pichai";
            marker.lat = "37.422160";
            marker.lng = "-122.084270";
            marker.icon = "google-logo.png";
            marker.title = "Google HQ";
            markerList.Add(marker);
            marker = new Models.GoogleModel();
            marker.ceo = "Jeff Bezos";
            marker.lat = "47.609920";
            marker.lng = "-122.339020";
            marker.icon = "amazon-logo.png";
            marker.title = "Amazon HQ";
            markerList.Add(marker);
            marker = new Models.GoogleModel();
            marker.ceo = "Satya Nadella";
            marker.lat = "47.639890";
            marker.lng = "-122.125090";
            marker.icon = "microsoft-logo.png";
            marker.title = "Microsoft HQ";
            markerList.Add(marker);
            marker = new Models.GoogleModel();
            marker.ceo = "Tim Cook";
            marker.lat = "37.333470";
            marker.lng = "-122.012000";
            marker.icon = "apple-logo.png";
            marker.title = "Apple HQ";
            markerList.Add(marker);
            marker = new Models.GoogleModel();
            marker.ceo = "Jeff Weiner";
            marker.lat = "37.423370";
            marker.lng = "-122.070930";
            marker.icon = "linkedin-logo.png";
            marker.title = "LinkedIn HQ";
            markerList.Add(marker);
            marker = new Models.GoogleModel();
            marker.ceo = "Mark Zuckerberg";
            marker.lat = "37.484379";
            marker.lng = "-122.148308";
            marker.icon = "facebook-logo.png";
            marker.title = "Facebook HQ";
            markerList.Add(marker);
            marker = new Models.GoogleModel();
            marker.ceo = "Marc Benioff";
            marker.lat = "37.789850";
            marker.lng = "-122.396800";
            marker.icon = "salesforce-logo.png";
            marker.title = "Salesforce HQ";
            markerList.Add(marker);
            return markerList;
        }
    }
}