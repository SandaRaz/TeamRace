using Evaluation.Models.Cnx;
using Evaluation.Models.Entity;
using Evaluation.Models.Utils;
using Evaluation.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Evaluation.Controllers
{
    public class PenaliteController : Controller
    {
        private readonly PsqlContext _psqlContext;
        private readonly IWebHostEnvironment _hostEnvironment;

        public PenaliteController(PsqlContext psqlContext, IWebHostEnvironment hostEnvironment)
        {
            _psqlContext = psqlContext;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult PenaliteHome(int currentPage, int offset, int signe, string order, string triColumn)
        {
            // ------------- tri et pagination --------------
            if (String.IsNullOrWhiteSpace(triColumn))
            {
                triColumn = "Id";
            }

            string nextOrder = "asc";
            if (!String.IsNullOrWhiteSpace(order))
            {
                if (order.Equals("asc")) nextOrder = "desc";
            }

            int limit = 7;
            int total = _psqlContext.Penalite.Count();

            int nextOffset = offset + (limit * signe);
            if (nextOffset < 0 || nextOffset >= total)
            {
                nextOffset = offset;
            }
            // ----------------------------------------------
            IQueryable<Penalite> penalitesQuery = _psqlContext.Penalite
                .Where(p => p.Etat != -1)
                .Include(p => p.Etape)
                .Include(p => p.Equipe)
                .Skip(nextOffset)
                .Take(limit);
            List<Penalite> penalites = Functions.Trier<Penalite>(penalitesQuery, triColumn, order);

            if (offset != nextOffset)
            {
                currentPage += signe;
            }

            ViewBag.offset = nextOffset;
            ViewBag.currentPage = currentPage;
            ViewBag.order = nextOrder;
            return View(penalites);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AjoutPenalite()
        {
            AjoutPenaliteViewModel viewModel = new AjoutPenaliteViewModel
            {
                Etapes = await _psqlContext.Etape.ToListAsync(),
                Equipes = await _psqlContext.Equipe.ToListAsync()
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AjoutPenalite(AjoutPenaliteViewModel viewModel)
        {
            // ModelState.AddModelError("TempsArriveeString", "Le format de l'heure est invalide.");
            try
            {
                TimeSpan tempsPenalite = Functions.ParseStringToTimeSpan(viewModel.TempsPenaliteString);
                Console.WriteLine($"=====> TEMP PENALITE: {tempsPenalite}");

                Penalite newPenalite = new Penalite
                {
                    EtapeId = Guid.Parse(viewModel.EtapeId),
                    EquipeId = Guid.Parse(viewModel.EquipeId),
                    TempsPenalite = tempsPenalite
                };

                await _psqlContext.Penalite.AddAsync(newPenalite);
                int writen = await _psqlContext.SaveChangesAsync();


                Console.WriteLine($"    NOMBRE DE LIGNE AFFECTEE  => {writen}");

            }
            catch(Exception e)
            {
                Console.Error.WriteLine(e.StackTrace);
                ViewBag.ErrorMessage = e.Message;
            }

            viewModel.Etapes = await _psqlContext.Etape.ToListAsync();
            viewModel.Equipes = await _psqlContext.Equipe.ToListAsync();

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<JsonResult> Delete(string id)
        {
            Console.WriteLine($"******* Try to Delete {id}");

            Guid guidId = Guid.Parse(id);
            int deleted = 0;

            try
            {
                Penalite? penaliteToDelete = await _psqlContext.Penalite
                        .Include(p => p.Etape)
                            .ThenInclude(e => e.Coureurs)
                        .FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));

                if (penaliteToDelete != null)
                {
                    penaliteToDelete.Etat = -1;
                    _psqlContext.Penalite.Update(penaliteToDelete);
                    deleted = await _psqlContext.SaveChangesAsync();

                    Console.WriteLine("Deleted =====> " + deleted);
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("ERROR: \n" + e.Message);
                return Json(new { result = false, error = e.Message });
            }

            return Json(new { result = true, affected = deleted });
        }
    }
}
