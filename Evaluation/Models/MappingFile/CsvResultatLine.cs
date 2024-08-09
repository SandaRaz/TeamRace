using CsvHelper.Configuration.Attributes;

namespace Evaluation.Models.MappingFile
{
    public class CsvResultatLine
    {
        [Index(0)]
        public string EtapeRang { get; set; }
        [Index(1)]
        public string NumeroDossard { get; set; }
        [Index(2)]
        public string Nom { get; set; }
        [Index(3)]
        public string Genre { get; set; }
        [Index(4)]
        public string DateNaissance { get; set; }
        [Index(5)]
        public string Equipe { get; set; }
        [Index(6)]
        public string Arrivee { get; set; }
    }
}
