namespace Evaluation.Models.ViewModel
{
    public class AddAdminViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string PictureName { get; set; }
        public IFormFile Picture { get; set; }
        public string Profil { get; set; }
        public DateTime DateCreation { get; set; }
        public bool KeepLoggedIn { get; set; }
    }
}
