using CsvHelper.Configuration.Attributes;

namespace Evaluation.Models.MappingFile
{
    public class CsvPointsLine
    {
        [Index(0)]
        public string Classement { get; set; }
        [Index(1)]
        public string Points { get; set; }
    }
}
