using Evaluation.Models.Cnx;
using Evaluation.Models.MappingView;
using Evaluation.Models.Utils;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Evaluation.Models.Entity
{
    public class Etape
    {
        private Guid _id = Guid.NewGuid();
        private string _nom = "";
        private string _lieu = "";
        private double _longueur;
        private int _nombreCoureur;
        private DateTime _dateDepart;
        private Guid _courseId;
        private Course _course;
        private int _rangEtape;
        private ICollection<Coureur> _coureurs;
        private ICollection<Resultat> _resultats;
        private ICollection<Penalite> _penalites;

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
        public string Lieu
        {
            get { return _lieu; }
            set { _lieu = value; }
        }
        public double Longueur
        {
            get { return _longueur; }
            set { _longueur = value; }
        }
        public int NombreCoureur
        {
            get { return _nombreCoureur; }
            set { _nombreCoureur = value; }
        }

        [Column(TypeName = "timestamp")]
        public DateTime DateDepart
        {
            get { return _dateDepart; }
            set { _dateDepart = value; }
        }

        [ForeignKey("Course")]
        public Guid CourseId
        {
            get { return _courseId; }
            set { _courseId = value; }
        }
        public Course Course
        {
            get { return _course; }
            set { _course = value; }
        }
        public int RangEtape
        {
            get { return _rangEtape; }
            set { _rangEtape = value; }
        }
        public ICollection<Coureur> Coureurs
        {
            get { return _coureurs; }
            set { _coureurs = value; }
        }
        public ICollection<Resultat> Resultats
        {
            get { return _resultats; }
            set { _resultats = value; }
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

        // ---------------- FUNCTIONS ------------------

        public List<Coureur> GetCoureurEquipe(PsqlContext context, string equipeId)
        {
            List<Coureur> coureurs = this.Coureurs.Where(c => c.EquipeId == Guid.Parse(equipeId)).ToList();

            return coureurs;
        }

        public async Task<int> AffecterCoureur(PsqlContext context, string coureurId)
        {
            Coureur? coureur = context.Coureur.FirstOrDefault(c => c.Id == Guid.Parse(coureurId));
            int nombreDansEtape = this.Coureurs.Where(c => c.Id == Guid.Parse(coureurId)).Count();
            int writen = 0;
            if (coureur != null && nombreDansEtape <= 0)
            {
                int nombreCoureurEquipePresent = this.Coureurs.Where(c => c.EquipeId == coureur.EquipeId).Count();
                if (nombreCoureurEquipePresent < this.NombreCoureur)
                {
                    this.Coureurs.Add(coureur);
                    writen = await context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("Nombre de coureur maximal pour cet etape est déja atteint");
                }
            }
            return writen;
        }

        public List<Classement> GetClassement(PsqlContext context, string order, string triColumn)
        {
            IQueryable<Classement> classementsQuery = context.Classement.Where(c => c.EtapeId == this.Id);
            List<Classement> classements = Functions.Trier<Classement>(classementsQuery, triColumn, order);

            return classements;
        }

        public List<Classement> GetClassement(PsqlContext context, string idEquipe, string order, string triColumn)
        {
            IQueryable<Classement> classementsQuery = context.Classement.Where(c => c.EtapeId == this.Id && c.EquipeId == Guid.Parse(idEquipe));
            List<Classement> classements = Functions.Trier<Classement>(classementsQuery, triColumn, order);

            return classements;
        }

        public async Task<List<ResultatAvecPenalite>> GetResultatsAvecPenalite(PsqlContext context)
        {
            var query = from rap in context.ResultatAvecPenalite
                        join e in context.Etape on rap.EtapeId equals e.Id
                        join c in context.Coureur on rap.CoureurId equals c.Id
                        where rap.EtapeId == this.Id
                        select new ResultatAvecPenalite
                        {
                            Id = rap.Id,
                            EtapeId = rap.EtapeId,
                            Etape = e,
                            CoureurId = rap.CoureurId,
                            Coureur = c,
                            DateArrivee = rap.DateArrivee,
                            DateArriveePenalisee = rap.DateArriveePenalisee
                        };
            return await query.ToListAsync();
        }
    }
}
