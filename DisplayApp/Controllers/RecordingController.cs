﻿using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System;

namespace DisplayApp.Controllers
{
    public class RecordingController : Controller
    {
        public IActionResult RecordingList()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> RecordingListData(string startDate, string endDate)
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

            using (var client = new HttpClient())
            {
                var apiUrl = $"https://www.015pbx.net/local/api/json/recording/recordings/list/?auth_username={username}&auth_password={password}&start={startTimestamp}&end={endTimestamp}";

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
