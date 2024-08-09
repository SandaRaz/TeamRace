using Evaluation.Models.Utils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Evaluation.Models.MappingView
{
    public class Classement
    {
        private Guid _etapeId;
        private string _nomEtape = "";
        private double _longueurEtape;
        private int _nombreCoureur;
        private DateTime _dateDepart;
        private int _rangEtape;
        private Guid _coureurId;
        private string _nomCoureur = "";
        private string _genreCoureur = "";
        private int _numDossard;
        private DateTime _dateArrivee;
        private DateTime _dateArriveePenalisee;
        private Guid _equipeId;
        private string _nomEquipe = "";
        private int _rang;
        private int _points;


        public Guid EtapeId
        {
            get { return _etapeId; }
            set { _etapeId = value; }
        }
        public string NomEtape
        {
            get { return _nomEtape; }
            set { _nomEtape = value; }
        }
        public double LongueurEtape
        {
            get { return _longueurEtape; }
            set { _longueurEtape = value; }
        }
        public int NombreCoureur
        {
            get { return _nombreCoureur; }
            set { _nombreCoureur = value; }
        }
        public DateTime DateDepart
        {
            get { return _dateDepart; }
            set { _dateDepart = value; }
        }
        public int RangEtape
        {
            get { return _rangEtape; }
            set { _rangEtape = value; }
        }
        public Guid CoureurId
        {
            get { return _coureurId; }
            set { _coureurId = value; }
        }
        public string NomCoureur
        {
            get { return _nomCoureur; }
            set { _nomCoureur = value; }
        }
        public string GenreCoureur
        {
            get { return _genreCoureur; }
            set { _genreCoureur = value; }
        }
        public int NumDossard
        {
            get { return _numDossard; }
            set { _numDossard = value; }
        }
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
        public Guid EquipeId
        {
            get { return _equipeId; }
            set { _equipeId = value; }
        }
        public string NomEquipe
        {
            get { return _nomEquipe; }
            set { _nomEquipe = value; }
        }
        public int Rang
        {
            get { return _rang; }
            set { _rang = value; }
        }
        public int Points
        {
            get { return _points; }
            set { _points = value; }
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

        public TimeSpan Chrono()
        {
            return this.DateArrivee - this.DateDepart;
        }
        public string ChronoString()
        {
            return Functions.TimeSpanToString(this.Chrono());
        }

        public TimeSpan Penalite()
        {
            return this.DateArriveePenalisee - this.DateArrivee;
        }

        public string PenaliteString()
        {
            return Functions.TimeSpanToString(this.Penalite());
        }

        public TimeSpan TempsFinal()
        {
            return this.DateArriveePenalisee - this.DateDepart;
        }

        public string TempsFinalString()
        {
            return Functions.TimeSpanToString(this.TempsFinal());
        }
    }
}
