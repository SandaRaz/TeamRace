namespace Evaluation.Models.ViewModel
{
    public class ImportMTDPViewModel
    {
        public IFormFile MaisonTravauxCsv { get; set; }
        public IFormFile DevisCsv { get; set; }
        public IFormFile PaimentCsv { get; set; }
    }
}
