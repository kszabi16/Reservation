using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Reservation.Service.Services
{
    public class ImageUploadService
    {
        // IDE ILLESZD BE AZ IMGBB OLDALRÓL KIMÁSOLT KULCSODAT!
        private readonly string _apiKey = "615863d065d0b81ca5492df883668c2a";

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            using var httpClient = new HttpClient();
            using var form = new MultipartFormDataContent();

            using var stream = file.OpenReadStream();
            var streamContent = new StreamContent(stream);
            form.Add(streamContent, "image", file.FileName);

            // Elküldjük a képet az ImgBB-nek
            var response = await httpClient.PostAsync($"https://api.imgbb.com/1/upload?key={_apiKey}", form);
            var responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Képfeltöltés sikertelen: " + responseString);
            }

            // Kinyerjük a publikus linket a válaszból
            using var jsonDocument = JsonDocument.Parse(responseString);
            var imageUrl = jsonDocument.RootElement.GetProperty("data").GetProperty("url").GetString();

            return imageUrl; // Ez egy tökéletes, azonnal használható .jpg link lesz!
        }
    }
}