using Evaluation.Models.Entity;
using Evaluation.Models.MappingView;

namespace Evaluation.Models.ViewModel
{
    public class EtapeClassementViewModel
    {
        public Etape Etape { get; set; }
        public List<Classement> Classements { get; set; }
    }
}
