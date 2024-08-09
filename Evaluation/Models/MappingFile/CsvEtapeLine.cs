using CsvHelper.Configuration.Attributes;

namespace Evaluation.Models.MappingFile
{
    public class CsvEtapeLine
    {
        [Index(0)]
        public string Etape { get; set; }
        [Index(1)]
        public string Longueur { get; set; }
        [Index(2)]
        public string NbCoureur { get; set; }
        [Index(3)]
        public string Rang { get; set; }
        [Index(4)]
        public string DateDepart { get; set; }
        [Index(5)]
        public string HeureDepart { get; set; }

    }
}
