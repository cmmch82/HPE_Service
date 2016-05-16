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


        // POST: api/championship/result
        [Route("api/championship/result")]
        public IHttpActionResult Post(string first, string second)
        {

            var service = new RPSService();
            service.ManualScoreEntry(first, second);
        
            return Ok();
        }
        [Route("api/championship/new")]
        public IEnumerable<Player> Post(HttpRequestMessage request)
        {

            var service = new RPSService();
            var data = request.Content.ReadAsStringAsync().Result;
            var answer = service.Championship(data);
            return answer;

        }

        // DELETE: api/championship/5
        public void Delete(int id)
        {
            var service = new RPSService();
            service.CleanDb();
        }
    }
}
