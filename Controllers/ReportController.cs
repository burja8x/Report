using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Report.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ReportController : Controller
    {
        [HttpGet]
        public IEnumerable<ReportRow> Get()
        {
            var n = Data.GetReportTable();
            return n.ToArray();
        }

        [HttpPost]
        public ActionResult<string> Post(IFormCollection collection)
        {
            Console.WriteLine("POST !!!!");
            Console.WriteLine();
            if (collection.Count < 2 || collection["content"].ToString().Length == 0)
            {   
                Console.WriteLine($"ERROR no data.{collection}");
                return $"ERROR no data.{collection}";
            }
            if (collection["sms"].ToString().Length == 9 || collection["mail"].ToString().Length > 9)
            {
                bool d = Data.InsertReport(collection["content"], true, collection["sms"], collection["mail"], $"processing...{Core.POD_UUID}");
                if (!d)
                {
                    return "NOT OK";
                }
            }
            else {
                Console.WriteLine("phone number must be length == 9. mail....");
                return "NOT OK x";
            }
            Console.WriteLine("insert done.");
            return "OK";
        }

        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        public IActionResult Post([FromForm] string data)
        {
            Console.WriteLine(data);
            return Json(data);
        }
        //http://20.52.212.120/report/api/v1/report
    }
}
