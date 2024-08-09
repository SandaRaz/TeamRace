namespace Evaluation.Models.Obj
{
    public class SelectListItem
    {
        public string Value { get; set; } = "";
        public string Libelle { get; set; } = "";

        public SelectListItem()
        {
        }

        public SelectListItem(string value, string libelle)
        {
            Value = value;
            Libelle = libelle;
        }
    }
}
