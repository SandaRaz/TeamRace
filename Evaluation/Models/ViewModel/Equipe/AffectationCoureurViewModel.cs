using Evaluation.Models.Entity;

namespace Evaluation.Models.ViewModel
{
    public class AffectationCoureurViewModel
    {
        public List<Coureur> Coureurs { get; set; } = new List<Coureur>();

        public List<Coureur> CoureurAffectes { get; set; } = new List<Coureur>();
        public Etape Etape { get; set; }
    }
}
