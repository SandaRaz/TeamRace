namespace Evaluation.Models.ViewModel.Truncates
{
    public class TruncateViewModel
    {
        public string Table { get; set; }

        public TruncateViewModel(string table)
        {
            Table = table;
        }
    }
}
