using Evaluation.Models.Cnx;
using Evaluation.Models.Utils;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Evaluation.Models.MappingFile
{
    public class CsvResultat
    {
        [Key]
        public Guid Id { get; set; }
        public int EtapeRang { get; set; }
        public int NumeroDossard { get; set; }
        public string Nom { get; set; }
        public string Genre { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime DateNaissance { get; set; }
        public string Equipe { get; set; }
        [Column(TypeName = "timestamp")]
        public DateTime Arrivee { get; set; }

        public ImportCsvResult<CsvResultat> GetCsvResult(List<CsvResultatLine> lines)
        {
            Console.WriteLine("Nombre csv line: " + lines.Count);

            List<CsvResultat> listEtapes = new List<CsvResultat>();
            List<LineError> lineErrors = new List<LineError>();
            foreach (CsvResultatLine line in lines)
            {
                try
                {
                    CsvResultat etape = new CsvResultat
                    {
                        Id = Guid.NewGuid(),
                        EtapeRang = Validation.ValidateInt(line.EtapeRang),
                        NumeroDossard = Validation.ValidateInt(line.NumeroDossard),
                        Nom = Validation.ValidateString(line.Nom),
                        Genre = Validation.ValidateString(line.Genre),
                        DateNaissance = Validation.FormatDate(line.DateNaissance.Trim()),
                        Equipe = Validation.ValidateString(line.Equipe),
                        Arrivee = Validation.FormatDate(line.Arrivee)
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
            return new ImportCsvResult<CsvResultat>(listEtapes, lineErrors);
        }

        public async Task<ImportCsvResult<CsvResultat>> DispatchToTableAsync(PsqlContext context, IWebHostEnvironment hostEnvironment, string csvFolder, IFormFile file)
        {
            Console.WriteLine("MIANTSO AN DISPATCH TO TABLE AN'I CSVPOINTS");

            ImportCsvResult<CsvResultat> result = new ImportCsvResult<CsvResultat>(new List<CsvResultat>(), new List<LineError>());
            bool uploaded = await Functions.UploadCsvFile(hostEnvironment, csvFolder, file);

            if (uploaded)
            {
                List<CsvResultatLine> resultatsFromCsv = Functions.ReadCsv<CsvResultatLine>(hostEnvironment, csvFolder, file.FileName);
                result = this.GetCsvResult(resultatsFromCsv);

                List<CsvResultat> csvResultats = result.ListeObject;

                context.CsvResultat.RemoveRange(context.CsvResultat);
                context.CsvResultat.AddRange(csvResultats);
                await context.SaveChangesAsync();

                var equipeSql = @"
                    INSERT INTO ""Equipe""(""Id"", ""Nom"", ""Email"", ""MotDePasse"", ""Profil"", ""DateCreation"")
                    SELECT uuid_generate_v4(),""Equipe"",""Equipe"",""Equipe"",'Equipe',Now()
                    FROM(
                        SELECT ""Equipe""
                        FROM ""CsvResultat""
                        GROUP BY ""Equipe""
                    ) as csveq
                    WHERE NOT EXISTS(
                        SELECT 1 FROM ""Equipe"" eq
                        WHERE eq.""Nom"" = csveq.""Equipe""
                    );
                ";

                var coureurSql = @"
                    INSERT INTO ""Coureur""(""Id"", ""Nom"", ""Genre"", ""DateNaissance"", ""NumDossard"", ""EquipeId"")
                    SELECT uuid_generate_v4(),""Nom"",""Genre"",""DateNaissance"",""NumeroDossard"",""EquipeId""
                    FROM(
                        WITH UniqueCoureur AS
                        (
                            SELECT ""NumeroDossard"", ""Nom"", ""Genre"", ""DateNaissance"", ""Equipe""
                            FROM ""CsvResultat""
                            GROUP BY ""NumeroDossard"", ""Nom"", ""Genre"", ""DateNaissance"", ""Equipe""
                        )
                        SELECT
                            uc.""Nom"",
                            uc.""Genre"",
                            uc.""DateNaissance"",
                            uc.""NumeroDossard"",
                            eq.""Id"" AS ""EquipeId""
                        FROM
                            UniqueCoureur uc
                        JOIN
                            ""Equipe"" eq ON uc.""Equipe"" = eq.""Nom""
                    ) AS csvcr
                    WHERE NOT EXISTS
                    (
                        SELECT 1 FROM ""Coureur"" cr
                        WHERE cr.""Nom"" = csvcr.""Nom"" AND cr.""NumDossard"" = csvcr.""NumeroDossard""
                    );
                ";

                var coureurEtapeSql = @"
                    INSERT INTO ""CoureurEtape""(""CoureurId"", ""EtapeId"")
                    SELECT ""CoureurId"",""EtapeId""
                    FROM(
                        SELECT
                            c.""Id"" AS ""CoureurId"",
                            et.""Id"" AS ""EtapeId""
                        FROM ""CsvResultat"" crs
                        JOIN ""Etape"" et ON crs.""EtapeRang"" = et.""RangEtape""
                        JOIN ""Coureur"" c ON crs.""NumeroDossard"" = c.""NumDossard""
                    ) AS csvce
                    WHERE NOT EXISTS
                    (
                        SELECT 1 FROM ""CoureurEtape"" ce
                        WHERE ce.""CoureurId"" = csvce.""CoureurId"" AND ce.""EtapeId"" = csvce.""EtapeId""
                    );
                ";

                var resultatSql = @"
                    INSERT INTO ""Resultat""(""Id"", ""EtapeId"", ""CoureurId"", ""DateArrivee"")
                    SELECT uuid_generate_v4(),""EtapeId"",""CoureurId"",""DateArrivee""
                    FROM(
                        SELECT
                            et.""Id"" AS ""EtapeId"",
                            c.""Id"" AS ""CoureurId"",
                            crs.""Arrivee"" AS ""DateArrivee""
                        FROM ""CsvResultat"" crs
                        JOIN ""Etape"" et ON crs.""EtapeRang"" = et.""RangEtape""
                        JOIN ""Coureur"" c ON crs.""NumeroDossard"" = c.""NumDossard""
                    ) AS csvrs
                    WHERE NOT EXISTS
                    (
                        SELECT 1 FROM ""Resultat"" rs
                        WHERE rs.""EtapeId"" = csvrs.""EtapeId"" AND rs.""CoureurId"" = csvrs.""CoureurId""
                    );
                ";

                context.Database.ExecuteSqlRaw(equipeSql);
                context.Database.ExecuteSqlRaw(coureurSql);
                context.Database.ExecuteSqlRaw(coureurEtapeSql);
                context.Database.ExecuteSqlRaw(resultatSql);

                await context.SaveChangesAsync();
            }

            return result;
        }
    }
}
