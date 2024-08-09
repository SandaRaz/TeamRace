using Evaluation.Models.Cnx;
using Evaluation.Models.MappingView;
using Evaluation.Models.Utils;
using Evaluation.Models.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Security.Claims;

namespace Evaluation.Models.Entity
{
    public class Equipe
    {
        private Guid _id = Guid.NewGuid();
        private string _nom = "";
        private string _email = "";
        private string _motDePasse = "";
        private string _profil = "";
        private DateTime _dateCreation = DateTime.Now;
        private ICollection<Coureur> _coureurs;
        private ICollection<Penalite> _penalites;
        
        [NotMapped]
        public int TempPoint { get; set; }
        [NotMapped]
        public int TempRang { get; set; }

        [Key]
        public Guid Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public string Nom
        {
            get { return _nom; }
            set {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new Exception("Veuillez entrer un nom valide");
                }
                _nom = value;
            }
        }
        public string Email
        {
            get { return _email; }
            set {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new Exception("Veuillez entrer un email valide");
                }
                _email = value;
            }
        }
        public string MotDePasse
        {
            get { return _motDePasse; }
            set {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new Exception("Veuillez entrer un mot de passe valide");
                }
                _motDePasse = value;
            }
        }
        public string Profil
        {
            get { return _profil; }
            set { _profil = value; }
        }

        [Column(TypeName = "timestamp")]
        public DateTime DateCreation
        {
            get { return _dateCreation; }
            set { _dateCreation = value; }
        }
        public ICollection<Coureur> Coureurs
        {
            get { return _coureurs; }
            set { _coureurs = value; }
        }

        public ICollection<Penalite> Penalites
        {
            get { return _penalites; }  
            set { _penalites = value; }
        }

        public override string? ToString()
        {
            string stringValue = "{";
            Type type = this.GetType();
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                stringValue += property.Name + "=" + property.GetValue(this) + ";";
            }
            stringValue += "}";

            return stringValue;
        }

        // ------------------ FUNCTION ---------------------

        public async Task<int> SignUpAsync(PsqlContext context, AddEquipeViewModel model)
        {
            if (!model.MotDePasse.Equals(model.ConfirmMotDePasse))
            {
                throw new Exception("Veuillez reconfirmer votre mot de passe");
            }

            Equipe equipe = new Equipe
            {
                Id = Guid.NewGuid(),
                Nom = model.Nom,
                Email = model.Email,
                MotDePasse = model.MotDePasse,
                Profil = "Equipe",
                DateCreation = DateTime.Now
            };

            await context.Equipe.AddAsync(equipe);
            int writen = await context.SaveChangesAsync();

            return writen;
        }

        public async Task<bool> SignInAsync(PsqlContext context, HttpContext httpContext, LoginEquipeViewModel model)
        {
            List<Equipe> equipes = await context.Equipe.Where(u => u.Email == model.Email).ToListAsync();
            Equipe? equipe = equipes.Find(u => u.MotDePasse == model.MotDePasse);

            if (equipe != null)
            {
                await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                string equipeId = equipe.Id.ToString();

                List<Claim> claims = new List<Claim>()
                {
                    new Claim("UserId", equipeId),
                    new Claim(ClaimTypes.Role, "Equipe"),
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                    IsPersistent = model.KeepLoggedIn
                };

                await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), properties);

                return true;
            }
            else
            {
                throw new Exception("Invalid email or password !");
            }
        }

        public List<ClassementEquipe> GetClassement(PsqlContext context, string order, string triColumn)
        {
            var classementsData = context.Classement
               .Where(c => c.EquipeId == this.Id)
               .Select(c => new
               {
                   c.CoureurId,
                   c.NomCoureur,
                   c.GenreCoureur,
                   c.NumDossard,
                   c.Points,
                   c.DateArriveePenalisee,
                   c.DateDepart
               })
               .ToList();

            IQueryable<ClassementEquipe> classementsQuery = classementsData
                .GroupBy(c => c.CoureurId)
                .Select(g => new ClassementEquipe
                {
                    CoureurId = g.Key,
                    NomCoureur = g.First().NomCoureur,
                    GenreCoureur = g.First().GenreCoureur,
                    NumDossard = g.First().NumDossard,
                    TotalPoints = g.Sum(c => c.Points),
                    TempsTotal = new TimeSpan(g.Sum(c => (c.DateArriveePenalisee - c.DateDepart).Ticks))
                }).AsQueryable();

            List<ClassementEquipe> classements = Functions.Trier<ClassementEquipe>(classementsQuery, triColumn, order);

            return classements;
        }

        public async Task<List<ClassementGeneraleEquipe>> GetClassementGenerales(PsqlContext context, string order, string triColumn)
        {
            List<ClassementGeneraleEquipe> classements = await context.ClassementEquipeCategorie
                .FromSqlRaw(@"SELECT * FROM ""ClassementEquipe""")
                .ToListAsync();

            return classements;
        }

        public async Task<List<ClassementGeneraleEquipe>> GetClassementEquipes(PsqlContext context, string categorieId, string order, string triColumn)
        {
            var sql = @"
                WITH RankedResults AS (
	                SELECT
		                rc.""CategorieId"",
                        rc.""EtapeId"",
		                rc.""CoureurId"",
		                rc.""EquipeId"" AS ""EquipeId"",
		                rc.""NomEquipe"",
		                rc.""DateArriveePenalisee"",
		                DENSE_RANK() OVER(PARTITION BY rc.""EtapeId"" ORDER BY rc.""DateArriveePenalisee"") AS ""Rang""
                    FROM
                        ""ResultatCategorie"" rc
                    WHERE
                        rc.""CategorieId"" = {0}
                ),
                RankedResultsWithPoints AS(
                    SELECT
                        rr.*,
		                COALESCE(pe.""Points"", 0) AS ""Points""
                    FROM
                        RankedResults rr
                    LEFT JOIN
                        ""PointEtape"" pe ON  rr.""Rang"" = pe.""Rang""
                ),
                ResultatParEquipe AS(
                    SELECT
                        rr.""EquipeId"",
		                rr.""NomEquipe"",
		                SUM(rr.""Points"") AS ""Points""
                    FROM
                        RankedResultsWithPoints rr
                    GROUP BY
                        rr.""EquipeId"",
		                rr.""NomEquipe""
                    ORDER BY
                        ""Points"" DESC
                )
                SELECT
                    e.""Id"" AS ""EquipeId"",
	                e.""Nom"" AS ""NomEquipe"",
	                COALESCE(re.""Points"", 0) AS ""Points"",
                    DENSE_RANK() OVER(ORDER BY COALESCE(re.""Points"", 0) DESC) AS ""Rang""
                FROM
                    ""Equipe"" e
                LEFT JOIN
                    ResultatParEquipe re ON e.""Id"" = re.""EquipeId"";
            ;";

            List<ClassementGeneraleEquipe> classements = await context.ClassementEquipeCategorie
                .FromSqlRaw(sql, Guid.Parse(categorieId))
                .ToListAsync();

            return classements;
        }

        public async Task<List<Equipe>> GetChampions(PsqlContext context)
        {
            List<ClassementGeneraleEquipe> classements = await GetClassementGenerales(context, "desc", "Points");
            int minRang = classements.Min(c => c.Rang);

            List<Equipe> champions = new List<Equipe>();
            foreach(ClassementGeneraleEquipe equipeClassee in classements.Where(c => c.Rang == minRang))
            {
                Equipe tempEquipe = context.Equipe.First(e => e.Id == equipeClassee.EquipeId);
                tempEquipe.TempPoint = equipeClassee.Points;
                tempEquipe.TempRang = equipeClassee.Rang;
                
                champions.Add(tempEquipe);
            }

            return champions;
        }

        public async Task<List<Equipe>> GetChampionsCategorie(PsqlContext context, string categorieId)
        {
            List<ClassementGeneraleEquipe> classements = await GetClassementEquipes(context, categorieId, "desc", "Points");
            int minRang = classements.Min(c => c.Rang);

            List<Equipe> champions = new List<Equipe>();
            foreach (ClassementGeneraleEquipe equipeClassee in classements.Where(c => c.Rang == minRang))
            {
                Equipe tempEquipe = context.Equipe.First(e => e.Id == equipeClassee.EquipeId);
                tempEquipe.TempPoint = equipeClassee.Points;
                tempEquipe.TempRang = equipeClassee.Rang;

                champions.Add(tempEquipe);
            }

            return champions;
        }
    }
}
