using CsvHelper.Configuration.Attributes;

namespace Evaluation.Models.Mapping
{
    public class PersonAccountLine
    {
        [Index(0)]
        public string Nom { get; set; }
        [Index(1)]
        public string Email { get; set; }
        [Index(2)]
        public string Password { get; set; }
        [Index(3)]
        public string DateCreation { get; set; }
        [Index(4)]
        public string TypeCompte { get; set; }
    }
}
