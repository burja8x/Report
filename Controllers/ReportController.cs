using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
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
            var lines = collection.Select(kvp => kvp.Key + ": " + kvp.Value.ToString());
            Log.Information("POST: " + string.Join(Environment.NewLine, lines));
            return HandlePost(collection);
        }

        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        public IActionResult Post([FromForm] string data)
        {
            var collection = Request.Form;
            var lines = collection.Select(kvp => kvp.Key + ": " + kvp.Value.ToString());
            Log.Information("POST2: " + string.Join(Environment.NewLine, lines));
            return Json(HandlePost(collection));
        }
        public string HandlePost(IFormCollection collection) {

            if (collection.Count < 2 || collection["content"].ToString().Length == 0)
            {
                //Log.Information("POST: " + string.Join(Environment.NewLine, lines));
                Log.Warning($"ERROR no data.{collection}");
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
            else
            {
                Log.Warning("phone number must be length == 9. mail....");
                return "NOT OK x";
            }
            Log.Information("Insert done.");
            return "OK";
        }
    }
}
