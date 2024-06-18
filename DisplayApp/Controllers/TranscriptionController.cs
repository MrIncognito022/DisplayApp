using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace DisplayApp.Controllers
{
    public class TranscriptionController : Controller
    {
        private readonly HttpClient _httpClient;

        public TranscriptionController()
        {
            _httpClient = new HttpClient();
        }
        public async Task<IActionResult> TranscribeAudio(int recordgroup, string uniqueid, string recordid)
        {
            var username = HttpContext.Session.GetString("username");
            var password = HttpContext.Session.GetString("password");

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            { 
                // If not authenticated, redirect to the login pagev
                return RedirectToAction("Login", "Auth");
            }

            // Prepare the API URL for playing the recording
            var apiUrl = $"https://www.015pbx.net/local/api/json/recording/recordings/get/?auth_username={username}&auth_password={password}&recordgroup={recordgroup}&uniqueid={uniqueid}&recordid={recordid}";

            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);

                try
                {
                    var response = await client.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content
                        var content = await response.Content.ReadAsStringAsync();
                        //return Ok(new { success = true, data = content });
                        var jsonResponse = JObject.Parse(content);
                        var nestedData = jsonResponse["data"]?["data"]?.ToString();
                        if (string.IsNullOrEmpty(nestedData))
                        {
                            return BadRequest("Enco dedAudioData is null or empty.");
                        }
                        try
                        {
                            // Decode the Base64 string
                            byte[] audioBytes = Convert.FromBase64String(nestedData);

                            // Save the audio file temporarily
                            string tempFilePath = Path.GetTempFileName() + ".wav";
                            await System.IO.File.WriteAllBytesAsync(tempFilePath, audioBytes);

                            // Call Azure Speech to Text service
                            string transcription = await GetTranscriptionFromAzure(tempFilePath);

                            // Delete the temporary file
                            System.IO.File.Delete(tempFilePath);

                            return Ok(new { transcription });
                        }
                        catch (Exception ex)
                        {
                            return StatusCode(500, new { message = "An error occurred while processing the request.", error = ex.Message });
                        }

                    }
                    else
                    {
                        // Handle unsuccessful response from the API
                        return BadRequest(new { success = false, message = "Error Getting recording" });
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
        private async Task<string> GetTranscriptionFromAzure(string filePath)
        {
            string subscriptionKey = "465e6b09667143dea5966343c991f323";
            string endpoint = "https://eastus.stt.speech.microsoft.com/speech/recognition/conversation/cognitiveservices/v1?language=he-IL";

            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            using (var audioFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                var content = new StreamContent(audioFileStream);
                content.Headers.ContentType = new MediaTypeHeaderValue("audio/wav");

                var response = await _httpClient.PostAsync(endpoint, content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var transcriptionResult = ExtractTranscriptionFromResponse(responseContent);
                return transcriptionResult;
            }
        }
        private string ExtractTranscriptionFromResponse(string responseContent)
        {
            // Parse the JSON response
            var jsonResponse = JObject.Parse(responseContent);
            return jsonResponse["DisplayText"]?.ToString();
        }
    }
}
