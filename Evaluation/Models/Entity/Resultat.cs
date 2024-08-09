using Evaluation.Models.Cnx;
using Evaluation.Models.Utils;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Evaluation.Models.Entity
{
    public class Resultat
    {
        private Guid _id = Guid.NewGuid();
        private Guid _etapeId;
        private Etape _etape;
        private Guid _coureurId;
        private Coureur _coureur;
        private DateTime _dateArrivee;
        // private int _rang; alaina avec Rank()


        [Key]
        public Guid Id
        {
            get { return _id; }
            set { _id = value; }
        }

        [ForeignKey("Etape")]
        public Guid EtapeId
        {
            get { return _etapeId; }
            set { _etapeId = value; }
        }
        public Etape Etape
        {
            get { return _etape; }
            set { _etape = value; }
        }
        [ForeignKey("Coureur")]
        public Guid CoureurId
        {
            get { return _coureurId; }
            set { _coureurId = value; }
        }
        public Coureur Coureur
        {
            get { return _coureur; }
            set { _coureur = value; }
        }
        [Column(TypeName = "timestamp")]
        public DateTime DateArrivee
        {
            get { return _dateArrivee; }
            set { _dateArrivee = value; }
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

        public TimeSpan GetTempsChrono()
        {
            return this.DateArrivee - this.Etape.DateDepart;
        }

        public string GetTempsChronoString()
        {
            return Functions.TimeSpanToString(this.GetTempsChrono());
        }

        // ---------------------- FUNCTIONS ----------------------

        public async Task<int> SaveResultat(PsqlContext context, string etapeId, string coureurId, DateTime dateArrivee)
        {
            int writen = 0;

            int possedeDejaResultat = await context.Resultat
                .Where(r => (r.EtapeId == Guid.Parse(etapeId) && r.CoureurId == Guid.Parse(coureurId))).CountAsync();


            Etape? etape = await context.Etape.FirstOrDefaultAsync(e => e.Id == Guid.Parse(etapeId));
            if(etape != null)
            {
                if(etape.DateDepart > dateArrivee)
                {
                    throw new Exception("Date arrive inferieur a Date de depart");
                }
            }

            if (possedeDejaResultat <= 0)
            {
                Resultat newResult = new Resultat
                {
                    Id = Guid.NewGuid(),
                    EtapeId = Guid.Parse(etapeId),
                    CoureurId = Guid.Parse(coureurId),
                    DateArrivee = dateArrivee
                };

                await context.Resultat.AddAsync(newResult);
                writen = await context.SaveChangesAsync();
                
            }

            return writen;
        }
    }
}
