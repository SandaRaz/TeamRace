using Evaluation.Models.Cnx;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Evaluation.Models.Entity
{
    public class Categorie
    {
        private Guid _id = Guid.NewGuid();
        private string _nom = "";
        private DateTime _dateCreation = DateTime.Now;
        private ICollection<Coureur> _coureurs;

        [Key]
        public Guid Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public string Nom
        {
            get { return _nom; }
            set { _nom = value; }
        }

        [Column(TypeName = "timestamp")]
        public DateTime DateCreation
        {
            get { return _dateCreation; }
            set { _dateCreation = value; }
        }
        public ICollection<Coureur> Coureurs
        {
            get { return _coureurs; }
            set { _coureurs = value; }
        }

        public override string? ToString()
        {
            string stringValue = "{";
            Type type = this.GetType();
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                stringValue += property.Name + "=" + property.GetValue(this) + ";";
            }
            stringValue += "}";

            return stringValue;
        }

        // ---------------- FUNCTIONS --------------------

        public async Task GenerateCategoriesCoureursAsync(PsqlContext context)
        {
            List<Coureur> coureurs = await context.Coureur.Include(c => c.Categories).ToListAsync();
            foreach(Coureur coureur in coureurs)
            {
                List<Categorie> possibleCategories = coureur.GetPossibleCategories(context);
                foreach(Categorie categorie in possibleCategories)
                {
                    if(!coureur.Categories.Any(c => c.Id == categorie.Id))
                    {
                        coureur.Categories.Add(categorie);
                    }
                }
            }

            int writen = await context.SaveChangesAsync();
            Console.WriteLine($"    ===> CATEGORIE AFFECTEE: {writen}");
        }
    }
}
