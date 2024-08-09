using Evaluation.Models.Entity;
using Evaluation.Models.Obj;

namespace Evaluation.Models.ViewModel
{
    public class EquipeHomeViewModel
    {
        public Equipe Equipe { get; set; }
        public List<CoureursEtape> CoureursEtapes { get; set; } = new List<CoureursEtape>();
    }
}
