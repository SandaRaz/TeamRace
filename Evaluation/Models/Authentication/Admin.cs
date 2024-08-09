using Evaluation.Models.Cnx;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Security.Claims;

namespace Evaluation.Models.Authentication
{
    public class Admin
    {
        private Guid _id;
        private string _name = "";
        private string _firstName = "";
        private DateTime _dateOfBirth;
        private string _email = "";
        private string _password = "";
        private string _pictureName = "no_image.jpg";
        private string _profil = "Admin";
        private DateTime _dateCreation = DateTime.Now;

        [NotMapped]
        public bool KeepLoggedIn { get; set; }

        [Key]
        public Guid Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        [Column(TypeName = "timestamp")]
        public DateTime DateOfBirth
        {
            get { return _dateOfBirth; }
            set { _dateOfBirth = value; }
        }
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
        public string PictureName
        {
            get { return _pictureName; }
            set { _pictureName = value; }
        }
        public string Profil
        {
            get { return _profil; }
            set { _profil = value; }
        }

        [Column(TypeName = "timestamp")]
        public DateTime DateCreation
        {
            get { return _dateCreation; }
            set { _dateCreation = value; }
        }


        public override string? ToString()
        {
            string stringValue = "{";
            Type type = typeof(Admin);
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                stringValue += property.Name + "=" + property.GetValue(this) + ";";
            }
            stringValue += "}";

            return stringValue;
        }

        public bool EmailExist(PsqlContext context)
        {
            Admin? user = context.Admin.Where(u => u.Email == this.Email).FirstOrDefault();
            return user != null;
        }

        public async Task<string> SaveAuthenticationInfoAsync(bool keepLoggedIn, HttpContext context)
        {
            if (!this.Profil.Equals("Admin"))
            {
                throw new Exception($"{this.Email} n'est pas un Administrateur");
            }
            this.KeepLoggedIn = keepLoggedIn;

            string userId = this.Id.ToString();

            List<Claim> claims = new List<Claim>()
            {
                new Claim("UserId", userId),
                new Claim(ClaimTypes.Role, "Admin"),
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = keepLoggedIn
            };

            await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), properties);

            return userId;
        }
    }
}
