namespace Evaluation.Models.ViewModel
{
    public class AddEquipeViewModel
    {
        public string Nom { get; set; } = "";
        public string Email { get; set; } = "";
        public string MotDePasse { get; set; } = "";
        public string ConfirmMotDePasse { get; set; } = "";
        public string Profil { get; set; } = "Equipe";
        public DateTime DateCreation { get; set; } = DateTime.Now;
    }
}
