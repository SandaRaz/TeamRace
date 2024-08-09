using Evaluation.Models.Cnx;
using Evaluation.Models.UnitTest;
using Evaluation.Models.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.Controllers
{
    public class UnitTestController : Controller
    {
        private readonly CustomLogger _logger;
        private readonly ConsoleInterceptor _interceptor;
        private readonly PsqlContext _psqlContext;
        public UnitTestController(PsqlContext psqlContext)
        {
            _psqlContext = psqlContext;
        }

        [HttpGet]
        public IActionResult Index(string[] args)
        {
            Console.WriteLine("--- UNIT TEST --- UNIT TEST --- UNIT TEST --- UNIT TEST --- ");

            // --------- Appeler ici les fonctions static Main() ---------
            Validation.Main(args);
            Functions.Main(args);
            MainTest.Main(args);
            // -----------------------------------------------------------
            Console.WriteLine("xxxxxx Test View Classement xxxxxx");
            Console.WriteLine($"Nombre de ligne de Classement: {_psqlContext.Classement.Count()}");

            Console.WriteLine("---- FIN TEST ---- FIN TEST --- FIN TEST ---- FIN TEST ---- ");

            return View();
        }

        public IActionResult Exemple(string[] args)
        {
            Console.SetOut(_interceptor);

            Console.WriteLine("Test de message intercepté");

            var consoleOutput = _interceptor.GetOutput();
            _logger.Log(consoleOutput);
            return View(_logger.GetLogMessages());
        }
    }
}
