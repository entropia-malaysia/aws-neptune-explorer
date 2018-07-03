using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using aws_neptune_explorer.Models;
using Microsoft.Extensions.Configuration;
using Gremlin.Net.Driver;
using Newtonsoft.Json;
using JsonFormatterPlus;
using Gremlin.Net.Structure.IO.GraphSON;

namespace aws_neptune_explorer.Controllers
{
    public class HomeController : Controller
    {
        IConfiguration _configuration;
        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Explorer()
        {
            return View();
        }

       
        public IActionResult Gremlin(string query)
        {
            try
            {
                var hostname = _configuration.GetSection("AzureCosmos").GetSection("HostName").Value;
                var port = int.Parse(_configuration.GetSection("AzureCosmos").GetSection("Port").Value);
                var authKey = _configuration.GetSection("AzureCosmos").GetSection("AuthKey").Value;
                var database = _configuration.GetSection("AzureCosmos").GetSection("Database").Value;
                var graph = _configuration.GetSection("AzureCosmos").GetSection("Graph").Value;

                var gremlinServer = new GremlinServer(hostname, port, enableSsl: true, username: "/dbs/" + database + "/colls/" + graph, password: authKey);
                var gremlinClient = new GremlinClient(gremlinServer, new GraphSON2Reader(), new GraphSON2Writer(), GremlinClient.GraphSON2MimeType);

                // var endpoint = _configuration.GetSection("AWS").GetSection("NeptuneEndpoint").Value;           
                // var gremlinServer = new GremlinServer(endpoint);  
                // var gremlinClient = new GremlinClient(gremlinServer);

            
                var users = gremlinClient.SubmitAsync<dynamic>(query).Result;

                //return Json(users);
                string res = JsonConvert.SerializeObject(users);
                string pre = JsonFormatter.Format(res);
                return Content(pre, "application/json");
            }
            catch(Exception ex)
            {
                string res = JsonConvert.SerializeObject(new {message=ex.Message,stackTrace=ex.StackTrace});
                string pre = JsonFormatter.Format(res);
                return Content(pre, "application/json");
            }
        }
    }
}
