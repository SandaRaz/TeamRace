using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Security.Claims;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Identity;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace Evaluation.Models.Utils
{
    public class Functions
    {

        public static string GenerateFileName(IFormFile file)
        {
            if (file == null)
            {
                return "no_image.jpg";
            }
            else
            {
                return Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            }
        }
        public static async void UpdloadImageAsync(IWebHostEnvironment hostEnvironment, string yourFolder, string imageName, IFormFile imageFile)
        {
            if (imageFile != null)
            {
                var wwwRootPath = hostEnvironment.WebRootPath;
                var imageUploadFolder = Path.Combine(wwwRootPath, yourFolder);
                if (!Directory.Exists(imageUploadFolder))
                {
                    Directory.CreateDirectory(imageUploadFolder);
                }
                var imageFilePath = Path.Combine(imageUploadFolder, imageName);
                using (var memoryStream = new MemoryStream())
                {
                    // Copy the file stream to memory stream
                    await imageFile.CopyToAsync(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    // Load the image from memory stream
                    using (var image = await Image.LoadAsync(memoryStream))
                    {
                        // Resize the image (e.g., to 2/3 of the original size)
                        int newWidth = (int)(image.Width * 2 / 3);
                        int newHeight = (int)(image.Height * 2 / 3);

                        image.Mutate(x => x.Resize(newWidth, newHeight));

                        // Save the image with compression
                        var encoder = new JpegEncoder
                        {
                            Quality = 75 // Adjust quality here
                        };

                        await image.SaveAsync(imageFilePath, encoder);
                    }
                }
            }
        }

        public static async void UpdloadImageAsyncOld(IWebHostEnvironment hostEnvironment, string yourFolder, string imageName, IFormFile imageFile)
        {
            if (imageFile != null)
            {
                var wwwRootPath = hostEnvironment.WebRootPath;
                var imageUploadFolder = Path.Combine(wwwRootPath, yourFolder);
                if (!Directory.Exists(imageUploadFolder))
                {
                    Directory.CreateDirectory(imageUploadFolder);
                }
                var imageFilePath = Path.Combine(imageUploadFolder, imageName);
                using (var fileStream = new FileStream(imageFilePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                    await fileStream.FlushAsync();
                }
            }
        }

        private static void TrimStringProperties<T>(T obj)
        {
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(string))
                {
                    var value = (string)property.GetValue(obj);
                    if (value != null)
                    {
                        value = value.Trim();
                        property.SetValue(obj, value);
                    }
                }
            }
        }


        // --------- CSV FILES ----------
        public static async Task<bool> UploadCsvFile(IWebHostEnvironment hostEnvironment, string yourFolder, IFormFile file)
        {
            try
            {
                string fileDir = Path.Combine(hostEnvironment.WebRootPath, yourFolder);
                if (!Directory.Exists(fileDir))
                {
                    Directory.CreateDirectory(fileDir);
                }
                string fileName = Path.Combine(fileDir, file.FileName);
                using (var fileStream = new FileStream(fileName, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                    await fileStream.FlushAsync();
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("ERROR UPLOAD: "+e.StackTrace);
                throw e;
            }
            return true;
        }

        public static bool CreateNewCsv<T>(IWebHostEnvironment hostEnvironment, string yourFolder, string fileName, List<T> lines)
        {
            var path = Path.Combine(hostEnvironment.WebRootPath, yourFolder, "New", fileName);
            using (var write = new StreamWriter(path))
            using (var csv = new CsvWriter(write, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords<T>(lines);
            }
            return true;
        }

        public static List<T> ReadCsv<T>(IWebHostEnvironment hostEnvironment, string csvFolder,string fileName)
        {
            List<T> csvLines = new List<T>();

            var path = Path.Combine(hostEnvironment.WebRootPath, csvFolder, fileName);
            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ","
            };
            Console.WriteLine($"PATH DU CSV {fileName} => {path}");
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, csvConfig))
            {
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    var csvLine = csv.GetRecord<T>();
                    TrimStringProperties(csvLine);
                    Console.WriteLine($"Add CSV LINE: {csvLine}");
                    csvLines.Add(csvLine);
                }
            }
            Console.WriteLine("**** NOMBRE FINAL ==>"+csvLines.Count);

            return csvLines;
        }

        // ------------------------------

        public static object? GetKeyValue(object obj,Type objType)
        {
            PropertyInfo[] properties = objType.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                bool isKey = property.IsDefined(typeof(KeyAttribute), inherit: false);
                if (isKey)
                {
                    return property.GetValue(obj);
                }
            }
            return null;
        }

        public static void AffectUpdatedField<T>(object from, object to)
        {
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                object? propValue = property.GetValue(from);
                if (propValue != null)
                {
                    if(propValue.GetType().IsEquivalentTo(typeof(DateTime)))
                    {
                        if (!propValue.Equals(DateTime.MinValue))
                        {
                            property.SetValue(to, propValue);
                        }
                    }else if (propValue.GetType().IsClass)
                    {
                        Console.WriteLine($"{property.Name} de {nameof(T)} est une class");
                        if(GetKeyValue(propValue,propValue.GetType()) != GetKeyValue(to, to.GetType()))
                        {
                            Console.WriteLine($"Valeur differente du precedent pour le champ {property.Name}");
                            property.SetValue(to, propValue);
                        }
                    }else
                    {
                        if (propValue != property.GetValue(to) && !String.IsNullOrWhiteSpace(propValue.ToString()))
                        {
                            property.SetValue(to, propValue);
                        }
                    }
                }
            }
        }

        public static List<T> Trier<T>(IQueryable<T> query, string triColumn, string orderType)
        {
            if (String.IsNullOrWhiteSpace(orderType))
            {
                return query.OrderBy(triColumn + " asc").ToList();
            }
            else
            {
                if (orderType.Equals("desc"))
                {
                    return query.OrderBy(triColumn + " desc").ToList();
                }
                else
                {
                    return query.OrderBy(triColumn + " asc").ToList();
                }
            }
        }

        public static string HashPassword(string word)
        {
            PasswordHasher<object> hasher = new PasswordHasher<object>();
            return hasher.HashPassword("", word);
        }
        public static bool VerifyPassword(string hashed,string provided)
        {
            PasswordHasher<object> hasher = new PasswordHasher<object>();
            PasswordVerificationResult result = hasher.VerifyHashedPassword("", hashed, provided);

            return result == PasswordVerificationResult.Success;
        }

        public static string GetAuthId(HttpContext httpContext, string role, string key)
        {
            string authId = "";
            
            ClaimsPrincipal claimEquipe = httpContext.User;
            if (claimEquipe.Identity != null)
            {
                if (claimEquipe.Identity.IsAuthenticated)
                {
                    if (claimEquipe.IsInRole(role))
                    {
                        authId = claimEquipe.FindFirstValue(key);
                    }
                }
            }

            return authId;
        }

        public static string TimeSpanToString(TimeSpan timeSpan)
        {
            int totalHours = (int)timeSpan.TotalHours;
            return string.Format("{0:D2}:{1:D2}:{2:D2}", totalHours, timeSpan.Minutes, timeSpan.Seconds);
        }

        public static TimeSpan ParseStringToTimeSpan(string timeString)
        {
            // Split the input string by ':'
            var timeParts = timeString.Split(':');

            // Ensure the input is valid
            if (timeParts.Length != 3)
            {
                throw new ArgumentException("Invalid time format. Expected format is HH:mm:ss");
            }

            // Parse the hours, minutes, and seconds
            int hours = int.Parse(timeParts[0]);
            int minutes = int.Parse(timeParts[1]);
            int seconds = int.Parse(timeParts[2]);

            // Calculate the days from hours
            int days = hours / 24;
            hours = hours % 24;

            // Create and return the TimeSpan
            return new TimeSpan(days, hours, minutes, seconds);
        }

        public static void Main(string[] args)
        {
            string password = "123";
            string hashed = HashPassword(password);
            Console.WriteLine($"True Password: {VerifyPassword(hashed, "123")}");

            Console.WriteLine("xxxxx TEST HASH PASSWORD xxxxx");
            Console.WriteLine($"requin en Hash >>> {HashPassword("requin")}");
            Console.WriteLine($"redhat en Hash >>> {HashPassword("redhat")}");
            Console.WriteLine($"tigre en Hash >>> {HashPassword("tigre")}");

            // ------ Utilisation Session --------
            /*
             * 
            public static void SetObject<T>(this ISession session, string key, T value)
            {
                session.SetString(key, JsonConvert.SerializeObject(value));
            }

            public static T GetObject<T>(this ISession session, string key)
            {
                var value = session.GetString(key);
                return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
            }


            HttpContext.Session.SetObject("NomDeLaCle", objetAStocker);
            var objetRécupéré = HttpContext.Session.GetObject < TypeDeL'Objet>("NomDeLaCle");

            *
            */
            // ----------------------------
        }
    }
}
