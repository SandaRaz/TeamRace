using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Evaluation.Models.Entity
{
    public class PointEtape
    {
        private Guid _id = Guid.NewGuid();
        private int _rang;
        private int _points;

        [Key]
        public Guid Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public int Rang
        {
            get { return _rang; }
            set { _rang = value; }
        }
        public int Points
        {
            get { return _points; }
            set { _points = value; }
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
