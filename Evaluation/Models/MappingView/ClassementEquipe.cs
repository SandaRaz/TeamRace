using Evaluation.Models.Utils;

namespace Evaluation.Models.MappingView
{
    public class ClassementEquipe
    {
        public Guid CoureurId { get; set; }
        public string NomCoureur { get; set; }
        public string GenreCoureur { get; set; }
        public int NumDossard { get; set; }
        public int TotalPoints { get; set; }
        public TimeSpan TempsTotal {get;set;}

        public string TotalTempsEnString()
        {
            return Functions.TimeSpanToString(this.TempsTotal);
        }
    }
}
