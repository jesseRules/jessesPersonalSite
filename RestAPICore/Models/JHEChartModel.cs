using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPICore.Models
{
    public class JHEChartModel
    {
        public List<ChartDataProvider> dataProvider { get; set; }
    }

    public class ChartDataProvider
    {
        public DateTime date_stamp { get; set; }
        public int cnt_confirmed { get; set; }
        public int cnt_death { get; set; }
        public int cnt_recovered { get; set; }
    }
}
