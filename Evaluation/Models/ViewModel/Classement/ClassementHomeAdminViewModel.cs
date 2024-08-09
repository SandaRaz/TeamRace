using Evaluation.Models.Entity;

namespace Evaluation.Models.ViewModel
{
    public class ClassementHomeAdminViewModel
    {
        public Course Course { get; set; }
        public List<Equipe> Equipes { get; set; } = new List<Equipe>();
        public List<Etape> Etapes { get; set; } = new List<Etape>();

        public string EquipeId { get; set; } = "";
        public string EtapeId { get; set; } = "";
    }
}
