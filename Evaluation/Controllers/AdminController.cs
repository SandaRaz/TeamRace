using Evaluation.Models.Authentication;
using Evaluation.Models.Cnx;
using Evaluation.Models.Entity;
using Evaluation.Models.Utils;
using Evaluation.Models.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace Evaluation.Controllers
{
    public class AdminController : Controller
    {
        private readonly PsqlContext _psqlContext;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly string _imageFolder = "upload/images";

        public AdminController(PsqlContext psqlContext, IWebHostEnvironment hostEnvironment)
        {
            this._psqlContext = psqlContext;
            this._hostEnvironment = hostEnvironment;
        }

        // -------------------------- AUTHENTICATION START --------------------------

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AdminSignUp()
        {
            AddAdminViewModel viewModel = new AddAdminViewModel();

            return View(viewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AdminSignUp(AddAdminViewModel viewModel)
        {
            try
            {
                if (!Validation.IsValidPassword(viewModel.Password) || !viewModel.Password.Equals(viewModel.ConfirmPassword))
                {
                    throw new Exception("Mot de passe invalide. Veuillez reconfirmer votre mot de passe");
                }
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                Admin user = new Admin()
                {
                    Id = Guid.NewGuid(),
                    Name = viewModel.Name,
                    FirstName = viewModel.FirstName,
                    DateOfBirth = viewModel.DateOfBirth,
                    Email = viewModel.Email,
                    Password = Functions.HashPassword(viewModel.Password),
                    PictureName = Functions.GenerateFileName(viewModel.Picture),
                    Profil = "Admin",
                    DateCreation = DateTime.Now
                };

                await _psqlContext.Admin.AddAsync(user);
                int written = await _psqlContext.SaveChangesAsync();

                if (written <= 0)
                {
                    ViewBag.ErrorMessage = "User non inseré !";
                }
                else
                {
                    Functions.UpdloadImageAsync(_hostEnvironment, _imageFolder, user.PictureName, viewModel.Picture);
                    await user.SaveAuthenticationInfoAsync(viewModel.KeepLoggedIn, HttpContext);

                    return RedirectToAction("AdminLogin");
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
        public IActionResult AdminLogin()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            if (claimUser.Identity != null)
            {
                if (claimUser.Identity.IsAuthenticated && claimUser.IsInRole("Admin"))
                {
                    string userId = claimUser.FindFirstValue("UserId");
                    Console.WriteLine($"Current User => {claimUser.Identity.Name}, Id => {userId}, Roles => {claimUser.IsInRole("Admin")}");

                    return RedirectToAction("Profil", "Admin");
                }
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AdminLogin(LoginAdminViewModel viewModel)
        {
            Console.WriteLine("     KeepLoggedIn: " + viewModel.KeepLoggedIn);
            try
            {
                List<Admin> users = await _psqlContext.Admin.Where(u => u.Email == viewModel.Email).ToListAsync();
                Admin? user = users.Find(u => Functions.VerifyPassword(u.Password, viewModel.Password));
                
                if (user != null)
                {
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                    await user.SaveAuthenticationInfoAsync(viewModel.KeepLoggedIn, HttpContext);

                    return RedirectToAction("Profil", "Admin");
                }
                else
                {
                    throw new Exception("Invalid email or password !");
                }
            }
            catch(Exception e)
            {
                Console.Error.WriteLine("ERROR: "+ e);
                ViewBag.ErrorMessage = e.Message;
                return View();
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Profil()
        {
            Admin? user = null;
            ClaimsPrincipal claimUser = HttpContext.User;
            if (claimUser.Identity != null)
            {
                if (claimUser.Identity.IsAuthenticated)
                {
                    if (claimUser.IsInRole("Admin"))
                    {
                        string id = claimUser.FindFirstValue("UserId");
                        user = await _psqlContext.Admin.FindAsync(Guid.Parse(id));
                    }
                    else if (claimUser.IsInRole("Client"))
                    {
                        user = new Admin
                        {
                            Name = "ClientAnonyme",
                            FirstName = "",
                            Email = claimUser.FindFirstValue("ClientPhoneNumber"),
                            DateOfBirth = DateTime.MinValue,
                            PictureName = "no_name"
                        };
                    }
                    
                }
            }

            if (user != null)
            {
                return View(user);
            }
            return RedirectToAction("AdminLogin");
        }

        public IActionResult AccessDenied()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            if(claimUser.Identity != null)
            {
                if (claimUser.Identity.IsAuthenticated)
                {
                    if (claimUser.IsInRole("Admin"))
                    {
                        return RedirectToAction("EquipeSignIn", "Equipe");
                    }
                    else if (claimUser.IsInRole("Equipe"))
                    {
                        return RedirectToAction("AdminLogin", "Admin");
                    }
                }
                else
                {
                    return RedirectToAction("EquipeSignIn", "Equipe");
                }
            }
            


            return View();
        }

        // ----------------------- END AUTHENTICATION ACTIONS -----------------------

        // ----------------------------- CRUD START ---------------------------------


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ListAll()
        {
            List<Admin> userList = await _psqlContext.Admin.ToListAsync();
            return View("List", userList);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult List(int currentPage, int offset, int signe, string order, string triColumn)
        {
            // ------------- tri et pagination --------------
            if (String.IsNullOrWhiteSpace(triColumn))
            {
                triColumn = "DateCreation";
            }

            string nextOrder = "asc";
            if (!String.IsNullOrWhiteSpace(order))
            {
                if (order.Equals("asc")) nextOrder = "desc";
            }

            int limit = 5;
            int total = _psqlContext.Admin.Count();

            int nextOffset = offset + (limit * signe);
            if(nextOffset < 0 || nextOffset >= total)
            {
                nextOffset = offset;
            }
            // ----------------------------------------------

            IQueryable<Admin> queryUserList = _psqlContext.Admin.FromSql($"SELECT * FROM \"Admin\" OFFSET { nextOffset } LIMIT { limit } ");
            List<Admin> userList = Functions.Trier<Admin>(queryUserList, triColumn, nextOrder);

            if(offset != nextOffset)
            {
                currentPage += signe;
            }

            ViewBag.offset = nextOffset;
            ViewBag.currentPage = currentPage;
            ViewBag.order = nextOrder;
            return View(userList);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public JsonResult ListJson(int currentPage, int offset, int signe, string order, string triColumn)
        {
            // ------------- tri et pagination --------------
            if (String.IsNullOrWhiteSpace(triColumn))
            {
                triColumn = "DateCreation";
            }

            string nextOrder = "asc";
            if (!String.IsNullOrWhiteSpace(order))
            {
                if (order.Equals("asc")) nextOrder = "desc";
            }

            int limit = 5;
            int total = _psqlContext.Admin.Count();

            int nextOffset = offset + (limit * signe);
            if (nextOffset < 0 || nextOffset >= total)
            {
                nextOffset = offset;
            }
            // ----------------------------------------------

            IQueryable<Admin> queryUserList = _psqlContext.Admin.FromSql($"SELECT * FROM \"Admin\" OFFSET { nextOffset } LIMIT { limit } ");
            List<Admin> userList = Functions.Trier<Admin>(queryUserList, triColumn, nextOrder);

            if (offset != nextOffset)
            {
                currentPage += signe;
            }

            ViewBag.offset = nextOffset;
            ViewBag.currentPage = currentPage;
            ViewBag.order = nextOrder;

            return Json(userList);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult SearchList(string keyword)
        {
            // split par un ou plus d'espace
            string[] keywords = Regex.Split(keyword, @"\s+");

            IQueryable<Admin> query = _psqlContext.Admin;
            List<Admin> results = new List<Admin>();

            try
            {
                foreach (string word in keywords)
                {
                    string loweredWord = word.ToLower();

                    var subQuery = query.Where(p =>
                        EF.Functions.Like(p.Profil.ToLower(), $"%{loweredWord}%"));

                    results.AddRange(subQuery);
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("ERROR: \n", e);
                throw new Exception(e.Message);
            }

            var noDoublons = results.Distinct().ToList();

            return Json(noDoublons);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult ListCard(int currentPage, int offset, int signe)
        {
            int limit = 5;
            int total = _psqlContext.Admin.Count();

            int nextOffset = offset + (limit * signe);
            if (nextOffset < 0 || nextOffset >= total)
            {
                nextOffset = offset;
            }
            List<Admin> userList = _psqlContext.Admin.FromSql($"SELECT * FROM \"Admin\" OFFSET { nextOffset } LIMIT { limit } ").ToList();
            if (offset != nextOffset)
            {
                currentPage += signe;
            }

            ViewBag.offset = nextOffset;
            ViewBag.currentPage = currentPage;
            return View(userList);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Update(string id)
        {
            Guid guidId = Guid.Parse(id);
            Admin? user = _psqlContext.Admin.Find(guidId);

            if (user != null)
            {
                UpdateAdminViewModel updateUser = new UpdateAdminViewModel
                {
                    Id = user.Id,
                    Name = user.Name,
                    FirstName = user.FirstName,
                    DateOfBirth = user.DateOfBirth,
                    Email = user.Email,
                    Password = user.Password,
                    PictureName = user.PictureName,
                    Profil = user.Profil,
                    DateCreation = user.DateCreation
                };

                return View(updateUser);
            }
            return RedirectToAction("List");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(UpdateAdminViewModel viewModel)
        {
            Admin updatedUser = new Admin
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                FirstName = viewModel.FirstName,
                DateOfBirth = viewModel.DateOfBirth,
                Email = viewModel.Email,
                Password = viewModel.Password,
                PictureName = viewModel.PictureName,
                Profil = viewModel.Profil,
                DateCreation = viewModel.DateCreation
            };

            try
            {
                Admin? currentUser = await _psqlContext.Admin.FindAsync(updatedUser.Id);
                if (currentUser != null)
                {
                    Functions.AffectUpdatedField<Admin>(updatedUser, currentUser);

                    Console.WriteLine("CURRENT User: " + currentUser.ToString());
                    _psqlContext.Admin.Update(currentUser);
                    await _psqlContext.SaveChangesAsync();

                    return RedirectToAction("List", new { currentPage = 1, offset = 0, signe = 0 });
                }
                return RedirectToAction("List", new { currentPage = 1, offset = 0, signe = 0 });
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("ERROR: \n ", e.StackTrace);
                ViewBag.ErrorMessage = e.Message;

                return View("Update", viewModel);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<JsonResult> Delete(string id)
        {
            Guid guidId = Guid.Parse(id);
            int deleted = 0;
            try
            {
                Admin? deleteUser = await _psqlContext.Admin.FindAsync(guidId);
                if (deleteUser != null)
                {
                    ClaimsPrincipal currentUser = HttpContext.User;
                    string currentUserId = currentUser.FindFirstValue("UserId");
                    if (id.Equals(currentUserId))
                    {
                        throw new Exception("Cannot delete current logged user !");
                    }

                    _psqlContext.Admin.Remove(deleteUser);
                    deleted = await _psqlContext.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("ERROR: \n"+ e.Message);
                return Json(new { result = false, error = e.Message });
            }

            return Json(new { result = true, affected = deleted });
        }

        // ------------------------------ CRUD END ----------------------------------


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ListEtapeAdmin(int currentPage, int offset, int signe, string order, string triColumn, string courseId)
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
                return RedirectToAction("Profil");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AffectationTempsCoureur(string etapeId)
        {
            Etape? etape = await _psqlContext.Etape
                .Include(e => e.Coureurs)
                    .ThenInclude(c => c.Equipe)
                .Include(e => e.Course)
                .Include(e => e.Resultats)
                    .ThenInclude(r => r.Coureur)
                .FirstOrDefaultAsync(e => e.Id == Guid.Parse(etapeId));
            if (etape == null)
            {
                TempData["ErrorMessage"] = $"Etape with Id {etapeId} was not found";
                return RedirectToAction("EquipeHome");
            }

            AffectationTempsCoureurViewModel viewModel = new AffectationTempsCoureurViewModel
            {
                Coureurs = etape.Coureurs.ToList(),
                ResultatEtapes = etape.Resultats.ToList(),
                Etape = etape
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AffectationTempsCoureur(string etapeId, string coureurId, DateTime dateArrivee)
        {
            Etape? etape = await _psqlContext.Etape
                .Include(e => e.Coureurs)
                    .ThenInclude(c => c.Equipe)
                .Include(e => e.Course)
                .Include(e => e.Resultats)
                    .ThenInclude(r => r.Coureur)
                .FirstOrDefaultAsync(e => e.Id == Guid.Parse(etapeId));
            if (etape == null)
            {
                TempData["ErrorMessage"] = $"Etape with Id {etapeId} was not found";
                return RedirectToAction("EquipeHome");
            }

            try
            {
                int writen = await new Resultat().SaveResultat(_psqlContext, etapeId, coureurId, dateArrivee);

                if (writen <= 0)
                {
                    throw new Exception("Resultat non enregistré");
                }

                AffectationTempsCoureurViewModel viewModel = new AffectationTempsCoureurViewModel
                {
                    Coureurs = etape.Coureurs.ToList(),
                    ResultatEtapes = etape.Resultats.ToList(),
                    Etape = etape
                };

                return View(viewModel);
            } 
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;

                AffectationTempsCoureurViewModel viewModel = new AffectationTempsCoureurViewModel
                {
                    Coureurs = etape.Coureurs.ToList(),
                    ResultatEtapes = etape.Resultats.ToList(),
                    Etape = etape
                };

                return View(viewModel);
            }
        }
    }
}
