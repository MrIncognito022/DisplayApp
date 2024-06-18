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
        [HttpPost]
        public async Task<IActionResult> TranscribeAudio(string encodedAudioData)
        {
            if (string.IsNullOrEmpty(encodedAudioData))
            { 
                return BadRequest("EncodedAudioData is null or empty.");
            }
            try
            {
                // Decode the Base64 string
                byte[] audioBytes = Convert.FromBase64String(encodedAudioData);

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
