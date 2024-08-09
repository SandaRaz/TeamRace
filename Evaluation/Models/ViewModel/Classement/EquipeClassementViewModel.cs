using Evaluation.Models.Entity;
using Evaluation.Models.MappingView;

namespace Evaluation.Models.ViewModel
{
    public class EquipeClassementViewModel
    {
        public Equipe Equipe { get; set; }
        public List<ClassementEquipe> Classements { get; set; }
    }
}
