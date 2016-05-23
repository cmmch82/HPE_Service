using HPE_Service.Ëntities;
using HPE_Service.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace HPE_Service.Controllers
{   [EnableCors("*", "*", "*")]
    public class Players
    {
        public string first { get; set; }
        public string second { get; set; }
    }

    public class ChampionshipController : ApiController
    {
        // GET: api/championship/top
        [Route("api/championship/top")]
        public IHttpActionResult Get(int count = 10)
        {

            var service = new RPSService();
            var answer = service.TopN(count);
            return Ok(answer);


        }

        public HttpResponseMessage Get(string filename = "game1")
        {

            string path = HttpContext.Current.Server.MapPath(string.Format("~/GameFiles/({0}).txt", filename));
           ;
            if (File.Exists(path))
            {

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                var stream = new FileStream(path, FileMode.Open);
                result.Content = new StreamContent(stream);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentDisposition.FileName = "game1.txt";
                return result;
            }
            return new HttpResponseMessage(HttpStatusCode.NotFound);


        }


        [Route("api/championship/result")]
        public IHttpActionResult Post([FromUri]Players results)
        {
            if (results == null)
            {
                return BadRequest("Please provide names for the winner and second place");
            }


            var service = new RPSService();

            service.ManualScoreEntry(results.first, results.second);


            return Ok();
        }


        [Route("api/championship/new")]
        public IHttpActionResult Post(HttpRequestMessage request)
        {

            if (request == null)
            {
                return BadRequest("Please provide the tournament data");
            }
            var service = new RPSService();
            var data = request.Content.ReadAsStringAsync().Result;
            var answer = service.Championship(data);
            return Ok(answer);

        }

        [Route("api/championship/narrator")]
        [HttpPost]
        public IHttpActionResult Narrator(HttpRequestMessage request)
        {

            if (request == null)
            {
                return BadRequest("Please provide the tournament data");
            }
            var service = new RPSService();
            var data = request.Content.ReadAsStringAsync().Result;
            var answer = service.Narrator(data);
            return Ok(answer);

        }
        [Route("api/championship/testsolution")]
        [HttpPost]
        public IHttpActionResult TestSolution(HttpRequestMessage request)
        {

            if (request == null)
            {
                return BadRequest("Please provide the tournament data");
            }
            try
            {
                var service = new RPSService();
                var data = request.Content.ReadAsStringAsync().Result;
                var answer = service.TestSolution(data);
                return Ok(answer);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }


        }

        // DELETE: api/championship/
        public IHttpActionResult Delete()
        {
            var service = new RPSService();
            service.CleanDb();
            return Ok();
        }
    }
}
