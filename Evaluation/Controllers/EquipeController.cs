using Evaluation.Models.Cnx;
using Evaluation.Models.Entity;
using Evaluation.Models.MappingView;
using Evaluation.Models.Obj;
using Evaluation.Models.Utils;
using Evaluation.Models.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;

namespace Evaluation.Controllers
{
    public class EquipeController : Controller
    {
        private readonly PsqlContext _psqlContext;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly string _imageFolder = "upload/images";

        public EquipeController(PsqlContext psqlContext, IWebHostEnvironment hostEnvironment)
        {
            this._psqlContext = psqlContext;
            this._hostEnvironment = hostEnvironment;

            ViewData["Layout"] = "_Equipe";
        }

        // -------------------------- AUTHENTICATION START --------------------------

        [HttpGet]
        [AllowAnonymous]
        public IActionResult EquipeSignUp()
        {
            AddEquipeViewModel viewModel = new AddEquipeViewModel();

            return View(viewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> EquipeSignUp(AddEquipeViewModel viewModel)
        {
            try
            {
                int writen = await new Equipe().SignUpAsync(_psqlContext, viewModel);
                if (writen > 0)
                {
                    return RedirectToAction("EquipeSignIn");
                }
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }

            return View(viewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult EquipeSignIn()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            if (claimUser.Identity != null)
            {
                if (claimUser.Identity.IsAuthenticated && claimUser.IsInRole("Equipe"))
                {
                    string userId = claimUser.FindFirstValue("UserId");
                    Console.WriteLine($"Current User => {claimUser.Identity.Name}, Id => {userId}, Roles => {claimUser.IsInRole("Admin")}");

                    return RedirectToAction("EquipeHome");
                }
            }

            LoginEquipeViewModel viewModel = new LoginEquipeViewModel();

            return View(viewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> EquipeSignIn(LoginEquipeViewModel viewModel)
        {
            try
            {
                bool valid = await new Equipe().SignInAsync(_psqlContext, HttpContext, viewModel);
                if (valid)
                {
                    return RedirectToAction("EquipeHome");
                }
            }catch(Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }
            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Equipe")]
        public async Task<JsonResult> LogOut()
        {
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("ERROR: \n", e);
                throw new Exception(e.Message);
            }
            return Json("success");
        }

        [HttpGet]
        [Authorize(Roles = "Equipe")]
        public async Task<IActionResult> ProfilEquipe()
        {
            string equipeId = Functions.GetAuthId(HttpContext, "Equipe", "UserId");
            Equipe? equipe = await _psqlContext.Equipe.FindAsync(Guid.Parse(equipeId));
            if (equipe == null)
            {
                return RedirectToAction("EquipeSignIn");
            }
            return View(equipe);
        }

        [HttpGet]
        [Authorize(Roles = "Equipe")]
        public async Task<IActionResult> EquipeHome()
        {
            Course? course = await _psqlContext.Course
                .Include(c => c.Etapes)
                    .ThenInclude(e => e.Resultats)
                .Include(c => c.Etapes)
                    .ThenInclude(e => e.Coureurs)

                .FirstOrDefaultAsync();
            if (course == null)
            {
                TempData["ErrorMessage"] = "Aucune course disponible dans la base";
                return RedirectToAction("ProfilEquipe");
            }

            string equipeId = Functions.GetAuthId(HttpContext, "Equipe", "UserId");
            Equipe? equipe = await _psqlContext.Equipe.FindAsync(Guid.Parse(equipeId));
            if (equipe == null)
            {
                return RedirectToAction("EquipeSignIn");
            }

            List<CoureursEtape> coureurEtapes = new List<CoureursEtape>();
            foreach(Etape etape in course.Etapes.OrderBy(e => e.RangEtape))
            {
                coureurEtapes.Add(new CoureursEtape
                {
                    Etape = etape,
                    CoureurAffectes = etape.GetCoureurEquipe(_psqlContext, equipeId)
                });
            }

            EquipeHomeViewModel viewModel = new EquipeHomeViewModel
            {
                Equipe = equipe,
                CoureursEtapes = coureurEtapes
            };

            return View(viewModel);
        }

        // -------------------------- AUTHENTICATION END ----------------------------


        [HttpGet]
        [Authorize(Roles = "Equipe")]
        public async Task<IActionResult> ListEtapeEquipe(int currentPage, int offset, int signe, string order, string triColumn, string courseId)
        {
            try
            {
                // ------------- tri et pagination --------------
                if (String.IsNullOrWhiteSpace(triColumn))
                {
                    triColumn = "RangEtape";
                }

                string nextOrder = "asc";
                if (!String.IsNullOrWhiteSpace(order))
                {
                    if (order.Equals("asc")) nextOrder = "desc";
                }

                int limit = 5;
                int total = _psqlContext.Etape.Where(e => e.CourseId == Guid.Parse(courseId)).Count();

                int nextOffset = offset + (limit * signe);
                if (nextOffset < 0 || nextOffset >= total)
                {
                    nextOffset = offset;
                }
                // ----------------------------------------------

                Course course = await _psqlContext.Course.FindAsync(Guid.Parse(courseId)) ?? throw new Exception($"Course with Id {courseId} was not found !");
                IQueryable<Etape> etapesQuery = _psqlContext.Etape.Where(e => e.CourseId == course.Id).Skip(nextOffset).Take(limit);
                List<Etape> etapes = Functions.Trier<Etape>(etapesQuery, triColumn, order);

                if (offset != nextOffset)
                {
                    currentPage += signe;
                }

                ViewBag.offset = nextOffset;
                ViewBag.currentPage = currentPage;
                ViewBag.order = nextOrder;

                EtapeEquipeViewModel viewModel = new EtapeEquipeViewModel
                {
                    Course = course,
                    Etapes = etapes
                };

                return View(viewModel);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.StackTrace);
                TempData["ErrorMessage"] = e.Message;
                return RedirectToAction("EquipeHome");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Equipe")]
        public async Task<IActionResult> AffectationCoureur(string etapeId)
        {
            string equipeId = Functions.GetAuthId(HttpContext, "Equipe", "UserId");
            Equipe? equipe = await _psqlContext.Equipe.Include(e => e.Coureurs).FirstOrDefaultAsync(e => e.Id == Guid.Parse(equipeId));
            if (equipe == null)
            {
                return RedirectToAction("EquipeSignIn");
            }

            Etape? etape = await _psqlContext.Etape
                .Include(e => e.Coureurs)
                .Include(e => e.Course)
                .FirstOrDefaultAsync(e => e.Id == Guid.Parse(etapeId));
            if (etape == null)
            {
                TempData["ErrorMessage"] = $"Etape with Id {etapeId} was not found";
                return RedirectToAction("EquipeHome");
            }

            AffectationCoureurViewModel viewModel = new AffectationCoureurViewModel
            {
                Coureurs = equipe.Coureurs.ToList(),
                CoureurAffectes = etape.GetCoureurEquipe(_psqlContext, equipeId),
                Etape = etape
            };

            return View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> AffectationCoureur(string coureurId, string etapeId)
        {
            string equipeId = Functions.GetAuthId(HttpContext, "Equipe", "UserId");
            Equipe? equipe = await _psqlContext.Equipe.Include(e => e.Coureurs).FirstOrDefaultAsync(e => e.Id == Guid.Parse(equipeId));
            if (equipe == null)
            {
                return RedirectToAction("EquipeSignIn");
            }

            Etape? etape = await _psqlContext.Etape
                .Include(e => e.Coureurs)
                .Include(e => e.Course)
                .FirstOrDefaultAsync(e => e.Id == Guid.Parse(etapeId));
            if (etape == null)
            {
                TempData["ErrorMessage"] = $"Etape with Id {etapeId} was not found";
                return RedirectToAction("EquipeHome");
            }

            try
            {
                int writen = await etape.AffecterCoureur(_psqlContext, coureurId);
                if (writen <= 0)
                {
                    throw new Exception("Coureur non affecté à l'étape");
                }

                AffectationCoureurViewModel viewModel = new AffectationCoureurViewModel
                {
                    Coureurs = equipe.Coureurs.ToList(),
                    CoureurAffectes = etape.GetCoureurEquipe(_psqlContext, equipeId),
                    Etape = etape
                };

                return View(viewModel);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;

                AffectationCoureurViewModel viewModel = new AffectationCoureurViewModel
                {
                    Coureurs = equipe.Coureurs.ToList(),
                    CoureurAffectes = etape.GetCoureurEquipe(_psqlContext, equipeId),
                    Etape = etape
                };

                return View(viewModel);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Equipe")]
        public async Task<IActionResult> ClassementToutCategorie(string courseId, string order, string triColumn)
        {
            ClaimsPrincipal claimsUser = HttpContext.User;
            if (claimsUser.Identity != null)
            {
                if (claimsUser.Identity.IsAuthenticated)
                {
                    if (claimsUser.IsInRole("Admin"))
                    {
                        ViewData["Layout"] = "_Admin";
                    }
                    else if (claimsUser.IsInRole("Equipe"))
                    {
                        ViewData["Layout"] = "_Equipe";
                    }
                }
            }

            Course course = await _psqlContext.Course.FirstAsync(c => c.Id == Guid.Parse(courseId));
            List<ClassementGeneraleEquipe> classements = await new Equipe().GetClassementGenerales(_psqlContext, order, triColumn);

            ClassementGeneraleEquipeViewModel viewModel = new ClassementGeneraleEquipeViewModel
            {
                Course = course,
                ClassementGeneraleEquipes = classements
            };

            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Equipe")]
        public async Task<IActionResult> ClassementParCategorie(string courseId, string categorieId, string order, string triColumn)
        {
            ClaimsPrincipal claimsUser = HttpContext.User;
            if (claimsUser.Identity != null)
            {
                if (claimsUser.Identity.IsAuthenticated)
                {
                    if (claimsUser.IsInRole("Admin"))
                    {
                        ViewData["Layout"] = "_Admin";
                    }
                    else if (claimsUser.IsInRole("Equipe"))
                    {
                        ViewData["Layout"] = "_Equipe";
                    }
                }
            }

            Course course = await _psqlContext.Course.FirstAsync(c => c.Id == Guid.Parse(courseId));
            Categorie categorie = await _psqlContext.Categorie.FirstAsync(c => c.Id == Guid.Parse(categorieId));
            List<ClassementGeneraleEquipe> classements = await new Equipe().GetClassementEquipes(_psqlContext, categorieId, order, triColumn);

            foreach(ClassementGeneraleEquipe classement in classements)
            {
                int isanyMitovyRang = classements.Where(c => c.Rang == classement.Rang).Count();
                if(isanyMitovyRang > 1)
                {
                    classement.Color = "rgba(0,200,0,1)";
                }
            }

            ClassementGeneraleEquipeViewModel viewModel = new ClassementGeneraleEquipeViewModel
            {
                Course = course,
                Categorie = categorie,
                ClassementGeneraleEquipes = classements
            };

            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Equipe")]
        public JsonResult RenvoyeVainqueur()
        {


            return Json(new { result = true });
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Equipe")]
        public async Task<JsonResult> ClassementGeneraleJson()
        {
            List<ClassementGeneraleEquipe> classements = new List<ClassementGeneraleEquipe>();
            try
            {
                classements = await new Equipe().GetClassementGenerales(_psqlContext, "desc", "Points");
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.StackTrace);
                return Json(new { result = false, error = e.Message });
            }
            return Json(new { result = true, classements = JsonSerializer.Serialize(classements) });
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Equipe")]
        public async Task<JsonResult> ClassementparCategorieJson(string categorieId)
        {
            Console.WriteLine($"    ==> CategorieId: {categorieId}");

            List<ClassementGeneraleEquipe> classements = new List<ClassementGeneraleEquipe>();
            try
            {
                classements = await new Equipe().GetClassementEquipes(_psqlContext, categorieId, "desc", "Points");
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.StackTrace);
                return Json(new { result = false, error = e.Message });
            }
            return Json(new { result = true, classements = JsonSerializer.Serialize(classements) });
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Equipe")]
        public async Task<JsonResult> GetChampionsGenerale()
        {
            try
            {
                List<Equipe> champions = await new Equipe().GetChampions(_psqlContext);

                return Json(new { result = true, champions = JsonSerializer.Serialize(champions) });
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.StackTrace);
                return Json(new { result = false, error = e.Message });
            }
            
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Equipe")]
        public async Task<JsonResult> GetChampionsParCategorie(string categorieId)
        {
            try
            {
                List<Equipe> champions = await new Equipe().GetChampionsCategorie(_psqlContext, categorieId);
                Categorie categorie = await _psqlContext.Categorie.FirstAsync(c => c.Id == Guid.Parse(categorieId));

                return Json(new { result = true, champions = JsonSerializer.Serialize(champions), categorie = JsonSerializer.Serialize(categorie) });
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.StackTrace);
                return Json(new { result = false, error = e.Message });
            }

        }
    }
}
