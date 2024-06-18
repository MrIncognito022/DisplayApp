using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> TranscribeAudio([FromBody] string encodedAudioData)
        {
            return Ok(encodedAudioData);
        }
    }
}
