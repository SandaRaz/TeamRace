using Evaluation.Models.Entity;

namespace Evaluation.Models.ViewModel
{
    public class AffectationTempsCoureurViewModel
    {
        public List<Coureur> Coureurs { get; set; } = new List<Coureur>();

        public List<Resultat> ResultatEtapes { get; set; } = new List<Resultat>();
        public Etape Etape { get; set; }
    }
}
