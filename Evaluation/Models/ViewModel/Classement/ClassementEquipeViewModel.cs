using Evaluation.Models.Entity;

namespace Evaluation.Models.ViewModel
{
    public class ClassementEquipeViewModel
    {
        public Course Course { get; set; }
        public List<Categorie> Categories { get; set; } = new List<Categorie>();
    }
}
