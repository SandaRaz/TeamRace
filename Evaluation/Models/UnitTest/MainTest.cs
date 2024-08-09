using Evaluation.Models.Utils;

namespace Evaluation.Models.UnitTest
{
    public class MainTest
    {
        public static void Main(string[] args)
        {
            Console.WriteLine($"\n NEW GUID ID 1: {Guid.NewGuid()} \n");
            Console.WriteLine($"\n NEW GUID ID 2: {Guid.NewGuid()} \n");
            Console.WriteLine($"\n NEW GUID ID 3: {Guid.NewGuid()} \n");
            Console.WriteLine($"\n NEW GUID ID 4: {Guid.NewGuid()} \n");
            Console.WriteLine($"\n NEW GUID ID 5: {Guid.NewGuid()} \n");
            Console.WriteLine($"\n NEW GUID ID 6: {Guid.NewGuid()} \n");


            Console.WriteLine(" ---- TEST DE TEMPS ----");

            DateTime dateDepart = Validation.FormatDate("2024-06-02 09:40:00");
            DateTime dateArrive = Validation.FormatDate("2024-06-02 11:34:00");

            TimeSpan tempsCourse = dateArrive - dateDepart;

            DateTime dateArrive2 = dateDepart + new TimeSpan(00, 10, 00);

            Console.WriteLine($"Temps de course Heure,minute,seconde: {tempsCourse:hh\\:mm\\:ss}");
            Console.WriteLine($"Date Arrivee 2 Heure,minute,seconde: {dateArrive2:dd-MM-yyyy HH:mm:ss}");

            // ----------------------------------------------------------------------------

            DateTime date1 = Validation.FormatDate("2024-06-01 13:39:00");
            DateTime date2 = Validation.FormatDate("2024-06-01 23:08:00");

            // Calculer la différence entre les deux DateTime
            TimeSpan tempsDeCourse = date2 - date1;

            // Obtenir la différence en minutes
            double minutes = tempsDeCourse.TotalMinutes;

            // Afficher la différence en minutes
            Console.WriteLine($"La différence en minutes est : {minutes} minutes");

            Console.WriteLine(" ---- TEST DE TEMPS ----");
        }
    }
}
