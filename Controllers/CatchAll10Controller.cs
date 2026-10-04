using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Text;

namespace CatchAll10API.Controllers
{
    [ApiController]
    [Route("{**path}")]
    public class CatchAll10Controller : ControllerBase
    {
        // No [HttpGet], [HttpPost], etc. => matches every HTTP verb
        // [HttpVerbs] could be used instead if using MVC
        public async Task<IActionResult> CatchAll()
        {
            // The ** token is available as a named route value
            var pathSegment = HttpContext.GetRouteData().Values["path"]?.ToString() ?? "";
            string extraInfo = "";
            if (Request.QueryString.HasValue)
            {
                extraInfo += $". QUERYSTRING: {Request.QueryString}";
            }

            //if a specific querystring parameter is specified, return special data
            // https://localhost:7200/demo/route/?dummyparam=1
            if (Request.Query.TryGetValue("dummyparam", out var dummyparam)
                    && dummyparam == "1")
            {
                Response.ContentType = MediaTypeNames.Application.Json;
      
                return Ok(new { Numberrr = 111, Texttt = "xxxyyyzzz", Boooolean = true, Arrrray = new int[] { 1, 2, 3, 4, 5 } });
            }
            else if (pathSegment.Contains("dummyjson"))
            {
                // http://localhost/CatchAllAPI/dummyjson/other?dummyparam=1
                Response.ContentType = MediaTypeNames.Application.Json;

                return Ok(new { Example = "123456789" });
            }
            else if (!string.Equals(Request.Method, "GET", StringComparison.OrdinalIgnoreCase))
            {
                //for all other requests, return a standard GET response
                using var reader = new StreamReader(Request.Body, Encoding.UTF8);
                var buffer = new char[32 * 1024]; //read just 32kb to prevent buffer overflows
                int charactersRead = await reader.ReadAsync(buffer.AsMemory(), HttpContext.RequestAborted);
                if (charactersRead > 0)
                {
                    extraInfo += $". BODY: {new string(buffer, 0, charactersRead)}";
                }
            }

            return Ok(new
            {
                message = $"All verbs caught at controller. Current verb: {Request.Method}{extraInfo}",
                method = Request.Method,
                path = Request.Path,
                pathSegment,
                contentType = Request.ContentType,
                contentLength = Request.ContentLength
            });
        }

        /*
        //override for a specific HTTP verb
        [HttpGet]
        public async Task<IActionResult> CatchAllGet()
        {
            return Ok(new
            {
                message = $"GET verb caught at controller.",
                method = Request.Method,
                path = Request.Path,
                contentType = Request.ContentType,
                contentLength = Request.ContentLength
            });
        }
        */
    }
}
