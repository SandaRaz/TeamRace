using Evaluation.Models.Cnx;
using Evaluation.Models.ViewModel.Truncates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Evaluation.Controllers
{
    [AllowAnonymous]
    public class TruncateController : Controller
    {
        private readonly PsqlContext _psqlContext;
        public TruncateController(PsqlContext psqlContext)
        {
            this._psqlContext = psqlContext;
        }

        public IActionResult Index()
        {
            List<string> tables = _psqlContext.ListAllTables();

            return View(tables);
        }

        public IActionResult TruncateTable(string table)
        {
            _psqlContext.Database.ExecuteSqlRaw($"DELETE FROM \"{table}\";");
            List<string> tables = _psqlContext.ListAllTables();

            return View("Index", tables);
        }

        public IActionResult TruncateAll()
        {
            List<string> tables = _psqlContext.ListAllTables();
            foreach(string table in tables)
            {
                _psqlContext.Database.ExecuteSqlRaw($"DELETE FROM \"{table}\";");
            }

            return View("Index", tables);
        }

    }
}
