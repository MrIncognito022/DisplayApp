using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace DisplayApp.Controllers
{
    public class CallsController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public CallsController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [HttpGet]
        public IActionResult CdrsList()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CdrListData(string startDate, string endDate)
        {
            var username = HttpContext.Session.GetString("username");
            var password = HttpContext.Session.GetString("password");

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return RedirectToAction("Login", "Auth");
            }

            if (string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate))
            {
                return Json(new { success = false, message = "Start date and end date are required" });
            }

            // Convert dates to Unix timestamps
            DateTime startDateTime = DateTime.Parse(startDate);
            DateTime endDateTime = DateTime.Parse(endDate);
            long startTimestamp = new DateTimeOffset(startDateTime).ToUnixTimeSeconds();
            long endTimestamp = new DateTimeOffset(endDateTime).ToUnixTimeSeconds();

            var request = new HttpRequestMessage(HttpMethod.Get,
                $"https://www.015pbx.net/local/api/json/cdrs/list/?auth_username={username};auth_password={password};start={startTimestamp};end={endTimestamp}");

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return Json(new { success = true, data = content });
            }
            else
            {
                return Json(new { success = false, message = "Error fetching data" });
            }
        }

    }
}
