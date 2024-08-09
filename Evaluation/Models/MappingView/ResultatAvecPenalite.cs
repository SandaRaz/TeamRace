using Evaluation.Models.Cnx;
using Evaluation.Models.Entity;
using Evaluation.Models.Utils;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Evaluation.Models.MappingView
{
    public class ResultatAvecPenalite
    {
        private Guid _id = Guid.NewGuid();
        private Guid _etapeId;
        private Etape _etape;
        private Guid _coureurId;
        private Coureur _coureur;
        private DateTime _dateArrivee;
        private DateTime _dateArriveePenalisee;
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
        public DateTime DateArriveePenalisee
        {
            get { return _dateArriveePenalisee; }
            set { _dateArriveePenalisee = value; }
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
    }
}
