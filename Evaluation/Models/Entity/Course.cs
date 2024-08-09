using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Evaluation.Models.Entity
{
    public class Course
    {
        private Guid _id = Guid.NewGuid();
        private string _nom = "";
        private DateTime _dateCourse;
        private TimeSpan _dureeHeure;
        private ICollection<Etape> _etapes;

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
        public DateTime DateCourse
        {
            get { return _dateCourse; }
            set { _dateCourse = value; }
        }
        public TimeSpan DureeHeure
        {
            get { return _dureeHeure; }
            set { _dureeHeure = value; }
        }
        public ICollection<Etape> Etapes
        {
            get { return _etapes; }
            set { _etapes = value; }
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

    }
}
