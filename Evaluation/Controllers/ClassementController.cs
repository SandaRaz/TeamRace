using Evaluation.Models.Cnx;
using Evaluation.Models.Entity;
using Evaluation.Models.MappingView;
using Evaluation.Models.Utils;
using Evaluation.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Evaluation.Controllers
{
    public class ClassementController : Controller
    {
        private readonly PsqlContext _psqlContext;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ClassementController(PsqlContext psqlContext, IWebHostEnvironment hostEnvironment)
        {
            _psqlContext = psqlContext;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Equipe")]
        public async Task<IActionResult> ClassementGeneraleParEtape(string courseId, string triColumn, string order)
        {
            if (String.IsNullOrWhiteSpace(triColumn))
            {
                triColumn = "Rang";
            }

            string nextOrder = "asc";
            if (!String.IsNullOrWhiteSpace(order))
            {
                if (order.Equals("asc")) nextOrder = "desc";
            }

            List<EtapeClassementViewModel> etapeClassements = new List<EtapeClassementViewModel>();

            List<Etape> etapes = await _psqlContext.Etape.Where(e => e.CourseId == Guid.Parse(courseId)).ToListAsync();
            foreach(Etape etape in etapes)
            {
                etapeClassements.Add(new EtapeClassementViewModel
                {
                    Etape = etape,
                    Classements = etape.GetClassement(_psqlContext, nextOrder, triColumn)
                });
            }

            Course course = await _psqlContext.Course.FirstAsync(c => c.Id == Guid.Parse(courseId));
            ViewBag.Course = course;

            ClaimsPrincipal claimsUser = HttpContext.User;
            if(claimsUser.Identity != null)
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
            return View(etapeClassements);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Equipe")]
        public async Task<IActionResult> ClassementGeneraleParEquipe(string courseId, string triColumn, string order)
        {
            if (String.IsNullOrWhiteSpace(triColumn))
            {
                triColumn = "TotalPoints";
            }

            string nextOrder = "asc";
            if (!String.IsNullOrWhiteSpace(order))
            {
                if (order.Equals("asc")) nextOrder = "desc";
            }

            List<EquipeClassementViewModel> equipeClassements = new List<EquipeClassementViewModel>();

            List<Equipe> equipes = await _psqlContext.Equipe.ToListAsync();
            foreach(Equipe equipe in equipes)
            {
                equipeClassements.Add(new EquipeClassementViewModel
                {
                    Equipe = equipe,
                    Classements = equipe.GetClassement(_psqlContext, nextOrder, triColumn)
                });
            }

            Course course = await _psqlContext.Course.FirstAsync(c => c.Id == Guid.Parse(courseId));
            ViewBag.Course = course;

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
            return View(equipeClassements);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ClassementHomeAdmin()
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
                return RedirectToAction("Profil", "Admin");
            }

            List<Equipe> equipes = await _psqlContext.Equipe.ToListAsync();
            List<Etape> etapes = await _psqlContext.Etape.ToListAsync();

            ClassementHomeAdminViewModel viewModel = new ClassementHomeAdminViewModel
            {
                Course = course,
                Equipes = equipes,
                Etapes = etapes
            };
            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Equipe")]
        public async Task<IActionResult> ClassementHomeEquipe()
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
                return RedirectToAction("Profil", "Admin");
            }

            List<Etape> etapes = await _psqlContext.Etape.ToListAsync();

            ClassementHomeEquipeViewModel viewModel = new ClassementHomeEquipeViewModel
            {
                Course = course,
                Etapes = etapes
            };
            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Equipe")]
        public async Task<IActionResult> ResultatClassementEquipe(string courseId, string equipeId, string triColumn, string order)
        {
            if (String.IsNullOrWhiteSpace(equipeId))
            {
                equipeId = Functions.GetAuthId(HttpContext, "Equipe", "UserId");
            }

            if (String.IsNullOrWhiteSpace(triColumn))
            {
                triColumn = "TotalPoints";
            }

            string nextOrder = "asc";
            if (!String.IsNullOrWhiteSpace(order))
            {
                if (order.Equals("asc")) nextOrder = "desc";
            }
            ViewBag.order = nextOrder;

            Course course = await _psqlContext.Course.FirstAsync(c => c.Id == Guid.Parse(courseId));
            ViewBag.Course = course;

            Equipe equipe = await _psqlContext.Equipe.FirstAsync(e => e.Id == Guid.Parse(equipeId));

            List<ClassementEquipe> classementEquipes = equipe.GetClassement(_psqlContext, order, triColumn);
            ResultatClassementViewModel viewModel = new ResultatClassementViewModel
            {
                Course = course,
                Equipe = equipe,
                ClassementEquipes = classementEquipes
            };

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

            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Equipe")]
        public async Task<IActionResult> ResultatClassementEtape(string courseId, string etapeId, string triColumn, string order)
        {
            if (String.IsNullOrWhiteSpace(triColumn))
            {
                triColumn = "Rang";
            }

            string nextOrder = "asc";
            if (!String.IsNullOrWhiteSpace(order))
            {
                if (order.Equals("asc")) nextOrder = "desc";
            }
            ViewBag.order = nextOrder;

            Course course = await _psqlContext.Course.FirstAsync(c => c.Id == Guid.Parse(courseId));
            ViewBag.Course = course;

            Etape etape = await _psqlContext.Etape.FirstAsync(e => e.Id == Guid.Parse(etapeId));

            List<Classement> classements = etape.GetClassement(_psqlContext, order, triColumn);
            ResultatClassementViewModel viewModel = new ResultatClassementViewModel
            {
                Course = course,
                Etape = etape,
                Classements = classements
            };

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

            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Equipe")]
        public async Task<IActionResult> ClassementEquipe()
        {
            string role = "";

            ClaimsPrincipal claimsUser = HttpContext.User;
            if (claimsUser.Identity != null)
            {
                if (claimsUser.Identity.IsAuthenticated)
                {
                    if (claimsUser.IsInRole("Admin"))
                    {
                        role = "Admin";
                        ViewData["Layout"] = "_Admin";
                    }
                    else if (claimsUser.IsInRole("Equipe"))
                    {
                        role = "Equipe";
                        ViewData["Layout"] = "_Equipe";
                    }
                }
            }

            Course? course = await _psqlContext.Course
                .Include(c => c.Etapes)
                    .ThenInclude(e => e.Resultats)
                .Include(c => c.Etapes)
                    .ThenInclude(e => e.Coureurs)
                .FirstOrDefaultAsync();

            if (course == null)
            {
                TempData["ErrorMessage"] = "Aucune course disponible dans la base";
                if (role.Equals("Admin"))
                {
                    return RedirectToAction("Profil", "Admin");
                }
                else if (role.Equals("Equipe"))
                {
                    return RedirectToAction("ProfilEquipe", "Equipe");
                }
                return RedirectToAction("EquipeSignIn", "Equipe");
            }

            List<Categorie> categories = await _psqlContext.Categorie.ToListAsync();
            ClassementEquipeViewModel viewModel = new ClassementEquipeViewModel
            {
                Course = course,
                Categories = categories
            };

            return View(viewModel);
        }
    }
}
