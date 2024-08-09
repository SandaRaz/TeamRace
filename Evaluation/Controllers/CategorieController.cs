using Evaluation.Models.Cnx;
using Evaluation.Models.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Evaluation.Controllers
{
    public class CategorieController : Controller
    {
        private readonly PsqlContext _psqlContext;
        private readonly IWebHostEnvironment _hostEnvironment;

        public CategorieController(PsqlContext psqlContext, IWebHostEnvironment hostEnvironment)
        {
            _psqlContext = psqlContext;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GenerationCategorie()
        {
            List<Categorie> categories = await _psqlContext.Categorie.ToListAsync();
            return View(categories);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GenerationCategorieCoureurs()
        {
            try
            {
                await new Categorie().GenerateCategoriesCoureursAsync(_psqlContext);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.StackTrace);
                ViewBag.ErrorMessage = e.Message;
            }

            List<Categorie> categories = await _psqlContext.Categorie.ToListAsync();
            return View("GenerationCategorie", categories);
        }
    }
}
