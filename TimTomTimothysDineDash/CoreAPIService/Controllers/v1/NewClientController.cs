using BarebonesRabbitMQImplementationLibrary.Helpers;
using BarebonesRabbitMQImplementationLibrary.Models;
using CoreAPIService.Helpers;
using CoreAPIService.Models.DTO.ClientApp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CoreAPIService.Controllers.v1
{
    /// <summary>
    /// This NewClientController class receives requests from devices who are just now using the Angular site.
    /// This provides information that can prove extremely informative for fixing problems without the hassle of asking the users.
    /// https://www.npmjs.com/package/ngx-device-detector - Uses this library for information pulling
    /// </summary>
    [Route("v{version:apiVersion}/[controller]/[action]")]
    [ApiVersion("1.0")]
    [ApiController]
    [Consumes("multipart/form-data")]
    public class NewClientController : ControllerBase
    {
        [HttpPut]
        public IActionResult ClientInformation([FromForm] ClientDeviceInfo clientDeviceInfo)
        {
            DataWithCorrelation dto = new();
            dto.ClientUUID = ControllerAids.SearchRequestForClientUUID(Request.Headers, dto.CorrelationGuid);
            Log.Information(LoggingHelper.FormatMessageWithCorrelationInformation(
                $"New client information reported: " +
                $"User Agent {clientDeviceInfo.UserAgent}; " +
                $"OS: {clientDeviceInfo.OS}; " +
                $"Browser: {clientDeviceInfo.Browser}; " +
                $"OS Version: {clientDeviceInfo.OSVersion}; " +
                $"Browser Version: {clientDeviceInfo.BrowserVersion}; " +
                $"Device Type: {clientDeviceInfo.DeviceType} " +
                $"Orientation: {clientDeviceInfo.Orientation}", dto));

            CookieOptions cookieOptions = new()
            {
                Expires = DateTimeOffset.UtcNow.AddDays(14), // Expires every fortnight, shows version changes better this way
                SameSite = SameSiteMode.Strict
            };

            Response.Cookies.Append("UserAssistInfoProvided", "true", cookieOptions);
            return Ok();
        }
    }
}
