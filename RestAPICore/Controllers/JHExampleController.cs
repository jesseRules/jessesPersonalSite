using Microsoft.AspNetCore.Mvc;
using RestAPICore.Services;
using System.Collections.Generic;

namespace RestAPICore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JHExampleController : ControllerBase
    {
        // FIPS https://transition.fcc.gov/oet/info/maps/census/fips/fips.txt#:~:text=FIPS%20codes%20are%20numbers%20which,to%20which%20the%20county%20belongs.
        // Source https://documenter.getpostman.com/view/2220438/SzYevv9u
        private readonly JHExampleService _jhExampleService;
        public JHExampleController()
        {
            _jhExampleService = new JHExampleService();
        }

        /// <summary>
        /// Returns data
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("data")]
        public Models.JHExampleModel GetByCounty(string county, int count, int offset)
        {
            return _jhExampleService.getCasesByCounty(county, offset, count);
        }

        /// <summary>
        /// Returns data
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("minimal")]
        public Models.JHEMinimal GetByCountyMinimal(string county, int count, int offset)
        {
            return _jhExampleService.getCasesByCountyMinimal(county, offset, count);
        }

        /// <summary>
        /// Returns data
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("chart")]
        public Models.JHEChartModel GetByCountyChart(string county, int count)
        {
            return _jhExampleService.getCasesByCountyChart(county, count);
        }


        /// <summary>
        /// Returns data
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("county/chartjs")]
        public Models.JHEChartJsModel GetByCountyChartJs(string county, int count)
        {
            return _jhExampleService.getCasesByCountyChartForChartJs(county, count);
        }

        /// <summary>
        /// Returns data by State
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("state/chartjs")]
        public Models.JHEChartJsModel GetByStateChartJs(string state, int count)
        {
            return _jhExampleService.getCasesByStateChartForChartJs(state, count);
        }

        /// <summary>
        /// Returns list of states
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("states")]
        public List<Models.StatesModel> GetStatesRef()
        {
            return _jhExampleService.getStates();
        }


    }
}