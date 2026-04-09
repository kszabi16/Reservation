using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Reservation.DataContext.Context;
using Reservation.DataContext.Entities;

namespace Reservation
{
    public static class DataSeeder
    {
        public static async Task SeedDataAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ReservationDbContext>();

            await context.Database.MigrateAsync();

            if (await context.Properties.CountAsync() >= 10)
                return;

            var host = await context.Users.FirstOrDefaultAsync(u => u.Role == RoleType.Host);
            if (host == null)
            {
                host = new User
                {
                    Username = "Példa Host",
                    Email = "host@pelda.hu",
                    PasswordHash = "TitkosJelszo123!",
                    Role = RoleType.Host,
                    IsTrustedHost = true,
                    CreatedAt = DateTime.UtcNow
                };
                context.Users.Add(host);
                await context.SaveChangesAsync();
            }

            var defaultAmenities = new[]
             {
                "Wifi", "Klíma", "Szauna", "Medence", "Ingyenes parkolás",
                "Okostévé", "Mosógép", "Konyha", "Erkély", "Kisállat bevihető",
                "Kandalló", "Jakuzzi", "Mosogatógép", "Kávéfőző", "Hajszárító",
                "Grillezési lehetőség", "Saját udvar", "Terasz", "Kerékpárkölcsönzés",
                "Infraszauna", "Saját partszakasz", "Edzőterem", "Társasjátékok",
                "Okosotthon rendszer", "Gyereksarok", "Bár"
            };
            foreach (var amenityName in defaultAmenities)
            {
                if (!await context.Amenities.AnyAsync(a => a.Name == amenityName))
                {
                    context.Amenities.Add(new Amenity { Name = amenityName });
                }
            }
            await context.SaveChangesAsync();
            var allAmenities = await context.Amenities.ToListAsync();

            var adjectives = new[]
            {
                "Panorámás", "Modern", "Luxus", "Hangulatos", "Romantikus",
                "Családbarát", "Erdei", "Vízparti", "Exkluzív", "Napfényes",
                "Elegáns", "Csendes", "Autentikus", "Rusztikus", "Minimalista",
                "Tóparti", "Központi", "Tágas", "Bohém", "Kétszintes",
                "Kutyabarát", "Prémium", "Bájos", "Történelmi", "Elszigetelt"
            };

            var types = new[]
            {
                "Apartman", "Vendégház", "Villa", "Stúdió", "Faház",
                "Nyaraló", "Kastély", "Lakás", "Birtok", "Kúria",
                "Glamping", "Jurta", "Penthouse", "Tanya", "Panzió",
                "Bungaló", "Lombház", "Vadászház"
            };

            var locations = new[]
            {
                "Budapest, I. kerület", "Budapest, V. kerület", "Budapest, VI. kerület", "Budapest, VII. kerület", "Budapest, XI. kerület",
                "Balatonfüred", "Siófok", "Veszprém", "Eger", "Sopron",
                "Pécs", "Győr", "Hévíz", "Zalakaros", "Szeged",
                "Debrecen", "Tihany", "Keszthely", "Zamárdi", "Badacsony",
                "Fonyód", "Szilvásvárad", "Lillafüred", "Visegrád", "Tokaj",
                "Noszvaj", "Kőszeg", "Gyula", "Szentendre", "Esztergom",
                "Zebegény", "Mátrafüred"
            };

            var descriptions = new[]
            {
                "Tökéletes választás a pihenni vágyóknak. Közel a központhoz, mégis csendes környezetben.",
                "Élvezze a páratlan kilátást és a prémium szolgáltatásokat ebben a gyönyörű szálláshelyen.",
                "Ideális családok és baráti társaságok számára. Teljesen felszerelt konyha és tágas terek várják.",
                "Szakadjon ki a hétköznapokból! A szállás minden kényelemmel rendelkezik a tökéletes feltöltődéshez.",
                "Frissen felújított, dizájnos belső térrel rendelkező szálláshely, karnyújtásnyira a legjobb éttermektől.",
                "Kivételes atmoszféra és stílusos berendezés várja vendégeinket ebben a különleges ingatlanban.",
                "Ha a természet lágy ölén szeretne pihenni, távol a város zajától, megtalálta a tökéletes helyet.",
                "Ébredjen madárcsicsergésre és élvezze a reggeli kávéját a lenyűgöző panorámát nyújtó teraszon.",
                "Pároknak kifejezetten ajánljuk: meghitt, romantikus fészek, ahol elbújhatnak a világ elől.",
                "Központi elhelyezkedésének köszönhetően a legfőbb látványosságok és szórakozóhelyek sétatávolságra találhatók.",
                "Gyerekbarát kialakítás: tágas udvar, trambulin és biztonságos környezet garantálja a kicsik szórakozását is.",
                "Prémium minőségű anyagok és exkluzív bútorok teszik felejthetetlenné az itt töltött napokat.",
                "Aktív pihenésre vágyóknak: rengeteg túraútvonal és sportolási lehetőség a közvetlen közelben.",
                "Történelmi környezet modern köntösben – élje át a múlt varázsát kompromisszumok nélkül.",
                "A hatalmas ablakokon keresztül egész nap dől be a napfény, ami csodálatos atmoszférát teremt a tágas nappaliban."
            };

            var realEstateImages = new[]
                 {
                    "https://images.unsplash.com/photo-1518780664697-55e3ad937233?auto=format&fit=crop&w=800&q=80", 
                    "https://images.unsplash.com/photo-1560448204-e02f11c3d0e2?auto=format&fit=crop&w=800&q=80", 
                    "https://images.unsplash.com/photo-1522708323590-d24dbb6b0267?auto=format&fit=crop&w=800&q=80",
                    "https://images.unsplash.com/photo-1600596542815-ffad4c1539a9?auto=format&fit=crop&w=800&q=80", 
                    "https://images.unsplash.com/photo-1512917774080-9991f1c4c750?auto=format&fit=crop&w=800&q=80", 
                    "https://images.unsplash.com/photo-1484154218962-a197022b5858?auto=format&fit=crop&w=800&q=80",
                    "https://images.unsplash.com/photo-1493809842364-78817add7ffb?auto=format&fit=crop&w=800&q=80", 
                    "https://images.unsplash.com/photo-1502672260266-1c1f55134b51?auto=format&fit=crop&w=800&q=80",
                    "https://images.unsplash.com/photo-1560185007-cde436f6a4d0?auto=format&fit=crop&w=800&q=80", 
                    "https://images.unsplash.com/photo-1564013799919-ab600027ffc6?auto=format&fit=crop&w=800&q=80", 
                    "https://images.unsplash.com/photo-1510798831971-661eb04b3739?auto=format&fit=crop&w=800&q=80", 
                    "https://images.unsplash.com/photo-1576013551627-0cc20b96c2a7?auto=format&fit=crop&w=800&q=80", 
                    "https://images.unsplash.com/photo-1568605114967-8130f3a36994?auto=format&fit=crop&w=800&q=80",
                    "https://images.unsplash.com/photo-1502005229762-cf1b2da7c5d6?auto=format&fit=crop&w=800&q=80", 
                    "https://images.unsplash.com/photo-1513694203232-719a280e022f?auto=format&fit=crop&w=800&q=80", 
                    "https://images.unsplash.com/photo-1540518614846-7eded433c457?auto=format&fit=crop&w=800&q=80", 
                    "https://images.unsplash.com/photo-1497366216548-37526070297c?auto=format&fit=crop&w=800&q=80", 
                    "https://images.unsplash.com/photo-1499916078039-922301b0eb9b?auto=format&fit=crop&w=800&q=80", 
                    "https://images.unsplash.com/photo-1533779283484-8ad4940aa3a8?auto=format&fit=crop&w=800&q=80", 
                    "https://images.unsplash.com/photo-1510627489930-0c1b0bfb6785?auto=format&fit=crop&w=800&q=80", 
                    "https://images.unsplash.com/photo-1552321554-5fefe8c9ef14?auto=format&fit=crop&w=800&q=80",
                    "https://images.unsplash.com/photo-1482731215275-a1f151646268?auto=format&fit=crop&w=800&q=80", 
                    "https://images.unsplash.com/photo-1556910103-1c02745aae4d?auto=format&fit=crop&w=800&q=80", 
                    "https://images.unsplash.com/photo-1449844908441-8829872d2607?auto=format&fit=crop&w=800&q=80"  
                };

            var random = new Random();
            var newProperties = new List<Property>();

            for (int i = 0; i < 50; i++)
            {
                var adjective = adjectives[random.Next(adjectives.Length)];
                var type = types[random.Next(types.Length)];
                var location = locations[random.Next(locations.Length)];
                var desc = descriptions[random.Next(descriptions.Length)];

                var property = new Property
                {
                    Title = $"{adjective} {type} - {location.Split(',')[0]}",
                    Description = $"{desc} Fedezze fel a környék szépségeit, miközben maximális kényelemben pihen. Az ingatlan kapacitása és elhelyezkedése garantálja a felejthetetlen élményt.",
                    Location = location,
                    PricePerNight = random.Next(15, 100) * 1000,
                    Capacity = random.Next(2, 12),
                    HostId = host.Id,
                    IsApproved = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-random.Next(1, 100)),
                    ImageUrl = realEstateImages[random.Next(realEstateImages.Length)]
                };

                newProperties.Add(property);
            }

            context.Properties.AddRange(newProperties);
            await context.SaveChangesAsync();

          
            foreach (var prop in newProperties)
            {
               
                var shuffledAmenities = allAmenities.OrderBy(x => random.Next()).Take(random.Next(3, 8)).ToList();
                foreach (var amenity in shuffledAmenities)
                {
                    context.PropertyAmenities.Add(new PropertyAmenity
                    {
                        PropertyId = prop.Id,
                        AmenityId = amenity.Id
                    });
                }
            }

           
            var comments = new[] { "Szuper hely, nagyon jól éreztük magunkat!", "Minden tiszta volt, a házigazda nagyon kedves.", "Kicsit drága, de megérte az árát.", "A kilátás pazar, visszatérünk még!", "Pont olyan, mint a képeken." };
            foreach (var prop in newProperties)
            {
                if (random.NextDouble() > 0.3) 
                {
                    int ratingCount = random.Next(1, 5);
                    for (int r = 0; r < ratingCount; r++)
                    {
                        context.Ratings.Add(new Rating
                        {
                            PropertyId = prop.Id,
                            UserId = host.Id, 
                            Score = random.Next(3, 6) 
                        });
                        context.Comments.Add(new Comment
                        {
                            PropertyId = prop.Id,
                            UserId = host.Id,
                            Content = comments[random.Next(comments.Length)],
                            CreatedAt = DateTime.UtcNow.AddDays(-random.Next(1, 30))
                        });
                    }
                }
            }

            await context.SaveChangesAsync();
        }
    }
}