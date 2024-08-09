using Evaluation.Models.Entity;
using Evaluation.Models.MappingView;

namespace Evaluation.Models.ViewModel
{
    public class ClassementGeneraleEquipeViewModel
    {
        public Course Course { get; set; }
        public Categorie Categorie { get; set; }
        public List<ClassementGeneraleEquipe> ClassementGeneraleEquipes { get; set; } = new List<ClassementGeneraleEquipe>();
    }
}
