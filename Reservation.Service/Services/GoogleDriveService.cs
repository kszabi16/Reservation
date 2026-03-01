using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Microsoft.AspNetCore.Http;

namespace Reservation.Service.Services
{
    public class GoogleDriveService
    {
        private readonly string[] Scopes = { DriveService.Scope.DriveFile };
        private readonly string ApplicationName = "ReservationApp";

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            // 1. Hitelesítés a letöltött JSON fájllal
            GoogleCredential credential;
            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
            }

            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // 2. Fájl metaadatok (Név és mappa beállítása)
            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName),
                // Ha akarsz külön mappát a Drive-on, ide kell a mappa ID-ja:
                // Parents = new List<string> { "MAPPA_ID_A_GOOGLE_DRIVE_BOL" } 
            };

            // 3. Fájl feltöltése
            Google.Apis.Drive.v3.Data.File uploadedFile;
            using (var stream = file.OpenReadStream())
            {
                var request = service.Files.Create(fileMetadata, stream, file.ContentType);
                request.Fields = "id";
                await request.UploadAsync();
                uploadedFile = request.ResponseBody;
            }

            // 4. Jogosultság beállítása: Bárki láthatja
            var permission = new Google.Apis.Drive.v3.Data.Permission()
            {
                Type = "anyone",
                Role = "reader"
            };
            await service.Permissions.Create(permission, uploadedFile.Id).ExecuteAsync();

            // 5. Visszaadjuk a KÖZVETLEN URL-t, amit az Angular <img> tag meg tud jeleníteni!
            return $"https://drive.google.com/uc?export=view&id={uploadedFile.Id}";
        }
    }
}