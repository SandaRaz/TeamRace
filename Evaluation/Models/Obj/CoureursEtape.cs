using Evaluation.Models.Entity;

namespace Evaluation.Models.Obj
{
    public class CoureursEtape
    {
        public Etape Etape { get; set; }
        public List<Coureur> CoureurAffectes { get; set; } = new List<Coureur>();
    }
}
