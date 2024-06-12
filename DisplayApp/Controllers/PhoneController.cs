using Microsoft.AspNetCore.Mvc;

namespace DisplayApp.Controllers
{
    public class PhoneController : Controller
    {

        [HttpGet]
        public IActionResult PhoneListPanel()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> PhoneListPanelData()
        {
            var username = HttpContext.Session.GetString("username");
            var password = HttpContext.Session.GetString("password");

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return RedirectToAction("Login", "Auth");
            }
            // Convert dates to Unix timestamps

            using (var client = new HttpClient())
            {
                var apiUrl = $"https://www.015pbx.net/local/api/json/phones/list/panel/?auth_username={username}&auth_password={password}";

                // Append call type to API URL if provided
                var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
                try
                {
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
                catch (HttpRequestException ex)
                {
                    // Log or handle the exception as needed
                    Console.WriteLine($"Request error: {ex.Message}");
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error connecting to API" });
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPhoneData(string name)
        {
            var username = HttpContext.Session.GetString("username");
            var password = HttpContext.Session.GetString("password");

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return RedirectToAction("Login", "Auth");
            }

            // Construct the API URL with the phone name
            var apiUrl = $"https://www.015pbx.net/local/api/json/phones/get/?auth_username={username}&auth_password={password}&name={name}";

            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);

                try
                {
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
                catch (HttpRequestException ex)
                {
                    // Log or handle the exception as needed
                    Console.WriteLine($"Request error: {ex.Message}");
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error connecting to API" });
                }
            }
        }


    }
}
