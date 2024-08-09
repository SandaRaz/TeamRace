using Evaluation.Models.Cnx;
using Evaluation.Models.Utils;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Evaluation.Models.MappingFile
{
    public class CsvPoints
    {
        [Key]
        public Guid Id { get; set; }
        public int Classement { get; set; }
        public int Points { get; set; }

        public ImportCsvResult<CsvPoints> GetCsvResult(List<CsvPointsLine> lines)
        {
            Console.WriteLine("Nombre csv line: " + lines.Count);

            List<CsvPoints> listPoints = new List<CsvPoints>();
            List<LineError> lineErrors = new List<LineError>();
            foreach (CsvPointsLine line in lines)
            {
                try
                {
                    CsvPoints points = new CsvPoints
                    {
                        Id = Guid.NewGuid(),
                        Classement = Validation.ValidateInt(line.Classement.Trim()),
                        Points = Validation.ValidateInt(line.Points.Trim())
                    };

                    listPoints.Add(points);
                }
                catch (Exception e)
                {
                    Console.WriteLine("----------------------------- ERROR -------------------------------");
                    Console.Error.WriteLine($"LINE PARSE ERROR => Ligne {lines.IndexOf(line) + 1}, {e.Message}");
                    Console.Error.WriteLine($"LINE PARSE ERROR => Ligne {lines.IndexOf(line) + 1}, {e.StackTrace}");
                    Console.WriteLine("----------------------------- ERROR -------------------------------");
                    lineErrors.Add(new LineError(lines.IndexOf(line) + 1, e.Message));
                    continue;
                }
            }
            return new ImportCsvResult<CsvPoints>(listPoints, lineErrors);
        }

        public async Task<ImportCsvResult<CsvPoints>> DispatchToTableAsync(PsqlContext context, IWebHostEnvironment hostEnvironment, string csvFolder, IFormFile file)
        {
            Console.WriteLine("MIANTSO AN DISPATCH TO TABLE AN'I CSVPOINTS");

            ImportCsvResult<CsvPoints> result = new ImportCsvResult<CsvPoints>(new List<CsvPoints>(), new List<LineError>());
            bool uploaded = await Functions.UploadCsvFile(hostEnvironment, csvFolder, file);

            if (uploaded)
            {
                List<CsvPointsLine> pointsFromCsv = Functions.ReadCsv<CsvPointsLine>(hostEnvironment, csvFolder, file.FileName);
                result = this.GetCsvResult(pointsFromCsv);

                List<CsvPoints> csvPoints = result.ListeObject;

                context.CsvPoints.RemoveRange(context.CsvPoints);
                context.CsvPoints.AddRange(csvPoints);
                await context.SaveChangesAsync();

                context.Database.ExecuteSqlRaw(@"
                    INSERT INTO ""PointEtape""(""Id"",""Rang"",""Points"")
                    SELECT uuid_generate_v4(),""Classement"", ""Points""
                    FROM(
                        SELECT ""Classement"", ""Points""
                        FROM ""CsvPoints""
                        GROUP BY ""Classement"", ""Points""
                    ) as csvpt
                    WHERE NOT EXISTS(
                        SELECT 1 FROM ""PointEtape"" pe
                        WHERE pe.""Rang"" = csvpt.""Classement""
                    );
                ");

                await context.SaveChangesAsync();
            }

            return result;
        }
    }
}
