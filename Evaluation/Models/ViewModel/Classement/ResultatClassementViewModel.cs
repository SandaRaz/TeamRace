using Evaluation.Models.Entity;
using Evaluation.Models.MappingView;

namespace Evaluation.Models.ViewModel
{
    public class ResultatClassementViewModel
    {
        public Course Course { get; set; }
        public Equipe Equipe { get; set; }
        public List<ClassementEquipe> ClassementEquipes { get; set; } = new List<ClassementEquipe>();
        public Etape Etape { get; set; }
        public List<Classement> Classements { get; set; } = new List<Classement>();
    }
}
