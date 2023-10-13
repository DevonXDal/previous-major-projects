using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CoreAPIService.Controllers
{
    /// <summary>
    /// This DefaultsController class handles routing for pages not specific to a particular area of functionality.
    /// This covers elements such as the swagger redirect, checking if the API is alive, and handling not found information.
    /// 
    /// Author: Devon X. Dalrymple
    /// </summary>
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class DefaultsController : ControllerBase
    {
        /// <summary>
        /// Redirects to the swagger documentation. To check if the API is available, use /Is-Alive instead: <see cref="IsAlive"/>.
        /// </summary>
        /// <returns>A redirect to swagger documentation</returns>
        [Route("/")]
        [Route("/api")]
        [Route("/docs")]
        public IActionResult Index()
        {
            Log.Information("A client has requested the swagger documentation");
            // From: https://stackoverflow.com/questions/49290683/how-to-redirect-root-to-swagger-in-asp-net-core-2-x
            return new RedirectResult("~/api/swagger");
        }

        /// <summary>
        /// Serves to help clients check whether they can reach the API.
        /// </summary>
        /// <returns>An Okay response confirming this web application is available without sending unnecessary information</returns>
        [Route("/Is-Alive")]
        [HttpGet]
        public IActionResult IsAlive()
        {
            return Ok();
        }

        /// <summary>
        /// Fallback route that helps when targeting the API.
        /// This is set up as invalid paths tend to redirect back to the SPA which
        /// is not intended.
        /// </summary>
        /// <returns>Not Found response (404), that shows the API is reachable and that the path is incorrect</returns>
        [Route("**")]
        public IActionResult NotFound()
        {
            return NotFound($"The route specified: '{Request.Path}', does not match any endpoints on the API");
        }
    }
}
