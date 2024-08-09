using Evaluation.Models.Entity;

namespace Evaluation.Models.ViewModel
{
    public class ClassementHomeEquipeViewModel
    {
        public Course Course { get; set; }
        public List<Etape> Etapes { get; set; } = new List<Etape>();
        public string EtapeId { get; set; } = "";
    }
}
