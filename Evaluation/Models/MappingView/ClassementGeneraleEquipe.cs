using System.ComponentModel.DataAnnotations.Schema;

namespace Evaluation.Models.MappingView
{
    public class ClassementGeneraleEquipe
    {
        public Guid EquipeId { get; set; }
        public string NomEquipe { get; set; } = "";
        public int Points { get; set; }
        public int Rang { get; set; }

        [NotMapped]
        public string Color { get; set; } = "rgba(30,30,30,1)";
    }
}
