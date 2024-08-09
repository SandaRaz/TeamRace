using Evaluation.Models.Cnx;
using Evaluation.Models.MappingView;
using Evaluation.Models.Utils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Evaluation.Models.Entity
{
    public class Coureur
    {
        private Guid _id = Guid.NewGuid();
        private string _nom;
        private string _genre;
        private DateTime _dateNaissance;
        private int _numDossard;
        private Guid _equipeid;
        private Equipe _equipe;
        private ICollection<Categorie> _categories;
        private ICollection<Etape> _etapes;
        private ICollection<Resultat> _resultats;
        //private ICollection<ResultatAvecPenalite> _resultats;

        [Key]
        public Guid Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public string Nom
        {
            get { return _nom; }
            set { _nom = value; }
        }
        public string Genre
        {
            get { return _genre; }
            set { _genre = value; }
        }

        [Column(TypeName = "timestamp")]
        public DateTime DateNaissance
        {
            get { return _dateNaissance; }
            set { _dateNaissance = value; }
        }


        public int NumDossard
        {
            get { return _numDossard; }
            set { _numDossard = value; }
        }

        [ForeignKey("Equipe")]
        public Guid EquipeId
        {
            get { return _equipeid; }
            set { _equipeid = value; }
        }
        public Equipe Equipe
        {
            get { return _equipe; }
            set { _equipe = value; }
        }
        public ICollection<Categorie> Categories
        {
            get { return _categories; }
            set { _categories = value; }
        }
        public ICollection<Etape> Etapes
        {
            get { return _etapes; }
            set { _etapes = value; }
        }
        public ICollection<Resultat> Resultats
        {
            get { return _resultats; }
            set { _resultats = value; }
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

        // ----------------- FUNCTIONS --------------------

        public TimeSpan GetTempsChronoEtape(Etape etape)
        {
            TimeSpan chrono = new TimeSpan(0, 0, 0);
            Resultat? resultatEtape = etape.Resultats
                .Where(r => (r.EtapeId == etape.Id && r.CoureurId == this.Id))
                .FirstOrDefault();

            if(resultatEtape != null)
            {
                chrono = resultatEtape.GetTempsChrono();
            }
            return chrono;
        }

        public DateTime GetDateArrive(Etape etape)
        {
            DateTime dateArrivee = new DateTime(0,0,0,0,0,0);
            Resultat? resultatEtape = etape.Resultats
                .Where(r => (r.EtapeId == etape.Id && r.CoureurId == this.Id))
                .FirstOrDefault();

            if (resultatEtape != null)
            {
                dateArrivee = resultatEtape.DateArrivee;
            }
            return dateArrivee;
        }

        public string GetTempsChronoEtapeString(Etape etape)
        {
            return Functions.TimeSpanToString(this.GetTempsChronoEtape(etape));
        }

        public List<Categorie> GetPossibleCategories(PsqlContext context)
        {
            Dictionary<string, List<string>> sexeCategories = new Dictionary<string, List<string>>();
            sexeCategories.Add("Homme", new List<string>
            {
                "M",
                "Garcon",
                "Masculin",
                "Masc",
                "Male",
                "Homme"
            });
            sexeCategories.Add("Femme", new List<string>
            {
                "F",
                "Fille",
                "Feminin",
                "Fem",
                "Femelle",
                "Femme"
            });

            List<Categorie> possibleCategories = new List<Categorie>();
            foreach (Categorie categorie in context.Categorie.ToList())
            {
                if (categorie.Nom.Equals("Junior"))
                {
                    if (DateTime.Now.Year - this.DateNaissance.Year < 18)
                    {
                        possibleCategories.Add(categorie);
                    }
                }
                else
                {
                    if (sexeCategories.ContainsKey(categorie.Nom))
                    {
                        List<string> sexeCategs = sexeCategories[categorie.Nom];
                        if (sexeCategs.Contains(this.Genre))
                        {
                            possibleCategories.Add(categorie);
                        }
                    }
                    
                }
            }

            return possibleCategories;
        }
    }
}
