using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPICore.Models
{
    public class GoogleModel
    {
        public string ceo { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public string title { get; set; }
        public string icon { get; set; }

        public bool Equals(GoogleModel modelX, GoogleModel modelY)
        {
            if (modelX == null || modelY == null)
            {
                return false;
            }
            if (modelX.ceo == modelY.ceo
            && modelX.lat == modelY.lat
            && modelX.lng == modelY.lng
            && modelX.title == modelY.title
            && modelX.icon == modelY.icon
            )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
