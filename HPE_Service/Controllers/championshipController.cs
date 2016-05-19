using HPE_Service.Ëntities;
using HPE_Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HPE_Service.Controllers
{
    public class ChampionshipController : ApiController
    {
        // GET: api/championship/top
        [Route("api/championship/top")]
        public IEnumerable<Player> Get( int count)
        {
            
            var service = new RPSService();
            var answer = service.TopN(count);
            return answer;

        }

        [HttpPost]
        public HttpResponseMessage Post([FromBody]string first, [FromBody]string second)
        {

            var service = new RPSService();
            service.ManualScoreEntry(first, second);

            HttpResponseMessage response = new HttpResponseMessage();
             response.StatusCode = HttpStatusCode.OK;


            return response;
        }
        [Route("api/championship/new")]
        public IEnumerable<Player> Post(HttpRequestMessage request)
        {

            var service = new RPSService();
            var data = request.Content.ReadAsStringAsync().Result;
            var answer = service.Championship(data);
            return answer;

        }

        // DELETE: api/championship/
        [Route("api/championship/truncate")]
        public void Delete()
        {
            var service = new RPSService();
            service.CleanDb();
        }
    }
}
