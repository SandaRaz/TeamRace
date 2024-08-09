using Evaluation.Models.MappingFile;
using Evaluation.Models.Utils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Evaluation.Models.Mapping
{
    public class PersonAccountCsv
    {
        [Key]
        public Guid Id { get; set; }
        public string Nom { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        [Column(TypeName = "timestamp")]
        public DateTime DateCreation { get; set; }
        public string TypeCompte { get; set; }


    }
}
