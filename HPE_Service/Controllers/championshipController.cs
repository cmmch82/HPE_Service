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

namespace HPE_Service.Controllers
{
    public class Players
    {
        public string first { get; set; }
        public string second { get; set; }
    }

    public class ChampionshipController : ApiController
    {
        // GET: api/championship/top
        //      public IEnumerable<Player> Get( int count = 10)
        [Route("api/championship/top")]
        public IHttpActionResult Get(int count = 10)
        {

            var service = new RPSService();
            var answer = service.TopN(count);
            return Ok(answer);


        }

        public HttpResponseMessage Get(string filename = "game1")
        {

            //var service = new RPSService();
            //var answer = service.TopN(count);
            //return Ok(answer);
        //    string path = HttpContext.Current.Server.MapPath("~/GameFiles/game1.txt");
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
                //return Ok(result);
            }
            return new HttpResponseMessage(HttpStatusCode.NotFound);
            //return BadRequest("Demo file not found sorry........");

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

            //HttpResponseMessage response = new HttpResponseMessage();
            //response.StatusCode = HttpStatusCode.OK;


            return Ok();
        }
        //[Route("api/championship/new")]
        //public IEnumerable<Player> Post(HttpRequestMessage request)
        //{

        //    var service = new RPSService();
        //    var data = request.Content.ReadAsStringAsync().Result;
        //    var answer = service.Championship(data);
        //    return answer;

        //}


        [Route("api/championship/new")]
        public IHttpActionResult Post([FromBody]string data)
        {

            if (data == null)
            {
                return BadRequest("Please provide the tournament data");
            }
            var service = new RPSService();
            var answer = service.Championship(data);
            return Ok(answer);

        }

        [Route("api/championship/narrator")]
        [HttpPost]
        public IHttpActionResult Narrator([FromBody]string data)
        {

            if (data == null)
            {
                return BadRequest("Please provide the tournament data");
            }
            var service = new RPSService();
            var answer = service.Narrator(data);
            return Ok(answer);

        }
        [Route("api/championship/testsolution")]
        [HttpPost]
        public IHttpActionResult TestSolution([FromBody]string data)
        {

            if (data == null)
            {
                return BadRequest("Please provide the tournament data");
            }
            var service = new RPSService();
            var answer = service.TestSolution(data);
            return Ok(answer);

        }

        // DELETE: api/championship/
        // [Route("api/championship/cleandb")]
        public IHttpActionResult Delete()
        {
            var service = new RPSService();
            service.CleanDb();
            return Ok();
        }
    }
}
//public HttpResponseMessage Get(string filename)
//{

//    //var service = new RPSService();
//    //var answer = service.TopN(count);
//    //return Ok(answer);
//    string path = HttpContext.Current.Server.MapPath("~/GameFiles/game1.txt");

//    if (File.Exists(path))
//    {

//        HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
//        var stream = new FileStream(path, FileMode.Open);
//        result.Content = new StreamContent(stream);
//        result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
//        result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
//        result.Content.Headers.ContentDisposition.FileName = "game1.txt";
//        return result;
//        //return Ok(result);
//    }
//    return new HttpResponseMessage(HttpStatusCode.NotFound);
//    //return BadRequest("Demo file not found sorry........");

//}