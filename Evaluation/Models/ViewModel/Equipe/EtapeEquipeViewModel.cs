using Evaluation.Models.Entity;

namespace Evaluation.Models.ViewModel
{
    public class EtapeEquipeViewModel
    {
        public Course Course { get; set; }
        public List<Etape> Etapes { get; set; } = new List<Etape>();
    }
}
