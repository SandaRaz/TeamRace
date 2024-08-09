using Evaluation.Models.Cnx;
using Evaluation.Models.Utils;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Evaluation.Models.MappingFile
{
    public class CsvEtape
    {
        [Key]
        public Guid Id { get; set; }
        public string Etape { get; set; }
        public double Longueur { get; set; }
        public int NbCoureur { get; set; }
        public int Rang { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime DateDepart { get; set; }

        public ImportCsvResult<CsvEtape> GetCsvResult(List<CsvEtapeLine> lines)
        {
            Console.WriteLine("Nombre csv line: " + lines.Count);

            List<CsvEtape> listEtapes = new List<CsvEtape>();
            List<LineError> lineErrors = new List<LineError>();
            foreach (CsvEtapeLine line in lines)
            {
                try
                {
                    CsvEtape etape = new CsvEtape
                    {
                        Id = Guid.NewGuid(),
                        Etape = Validation.ValidateString(line.Etape),
                        Longueur = Validation.ValidateDouble(line.Longueur.Trim()),
                        NbCoureur = Validation.ValidateInt(line.NbCoureur.Trim()),
                        Rang = Validation.ValidateInt(line.Rang.Trim()),
                        DateDepart = Validation.FormatDate($"{line.DateDepart.Trim()} {line.HeureDepart.Trim()}")
                    };

                    listEtapes.Add(etape);
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
            return new ImportCsvResult<CsvEtape>(listEtapes, lineErrors);
        }

        public async Task<ImportCsvResult<CsvEtape>> DispatchToTableAsync(PsqlContext context, IWebHostEnvironment hostEnvironment, string csvFolder, IFormFile file)
        {
            Console.WriteLine("MIANTSO AN DISPATCH TO TABLE AN'I CSVPOINTS");

            ImportCsvResult<CsvEtape> result = new ImportCsvResult<CsvEtape>(new List<CsvEtape>(), new List<LineError>());
            bool uploaded = await Functions.UploadCsvFile(hostEnvironment, csvFolder, file);

            if (uploaded)
            {
                List<CsvEtapeLine> etapesFromCsv = Functions.ReadCsv<CsvEtapeLine>(hostEnvironment, csvFolder, file.FileName);
                result = this.GetCsvResult(etapesFromCsv);

                List<CsvEtape> csvEtapes = result.ListeObject;

                context.CsvEtape.RemoveRange(context.CsvEtape);
                context.CsvEtape.AddRange(csvEtapes);
                await context.SaveChangesAsync();

                context.Database.ExecuteSqlRaw(@"
                    INSERT INTO ""Etape""(""Id"", ""Nom"", ""Lieu"", ""Longueur"", ""NombreCoureur"", ""DateDepart"", ""CourseId"", ""RangEtape"")
                    SELECT uuid_generate_v4(), ""Etape"", '', ""Longueur"", ""NbCoureur"", ""DateDepart"", '1a785405-e30a-4617-9761-a638856079b6', ""Rang""
                    FROM(
                        WITH EtapeRang AS
                        (
                            SELECT ""Etape"", ""Rang""
                            FROM ""CsvEtape""
                            GROUP BY ""Etape"", ""Rang""
                        )
                        SELECT
                            er.""Etape"",
                            ce.""Longueur"",
                            ce.""NbCoureur"",
                            er.""Rang"",
                            ce.""DateDepart""
                        FROM
                            EtapeRang er
                        LEFT JOIN
                            ""CsvEtape"" ce ON er.""Etape"" = ce.""Etape"" AND er.""Rang"" = ce.""Rang""
                    ) as csvetp
                    WHERE NOT EXISTS(
                        SELECT 1 FROM ""Etape"" et
                        WHERE et.""Nom"" = csvetp.""Etape""
                    );
                ");

                await context.SaveChangesAsync();
            }

            return result;
        }
    }
}
