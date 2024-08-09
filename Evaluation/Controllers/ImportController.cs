using Evaluation.Models.Cnx;
using Evaluation.Models.MappingFile;
using Evaluation.Models.ViewModel;
using Evaluation.Models.ViewModel.Import;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.Controllers
{
    public class ImportController : Controller
    {
        private readonly PsqlContext _psqlContext;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly string _csvFolder = "upload/csv";

        public ImportController(PsqlContext psqlContext,IWebHostEnvironment hostEnvironment)
        {
            _psqlContext = psqlContext;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ImportCsv()
        {
            return View();
        }

        /*
        [HttpPost]
        public async Task<JsonResult> ImportCsvJson(IFormFile file)
        {
            ImportCsvResult? result = null;
            try
            {
                bool uploaded = await Functions.UploadCsvFile(_hostEnvironment, _csvFolder, file);
                if (uploaded)
                {
                    List<PersonAccountLine> personAccountLines = Functions.ReadCsv<PersonAccountLine>(_hostEnvironment, _csvFolder, file.FileName);
                    result = new PersonAccountCsv().ImportCsvToDatabase(personAccountLines);
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("ERROR: \n", e);
                ViewBag.ErrorMessage = e.Message;
            }
            Console.WriteLine("Resultat retournés");
            return Json(result);
        }
        */
        // --------------------------------------------------------------
        

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult ValidationImport()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult ImportationEtapeResultat()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ImportationEtapeResultat(ImportationViewModel viewModel)
        {

            using (var transaction = _psqlContext.Database.BeginTransaction())
            {
                try
                {
                    if(viewModel.EtapesFile == null || viewModel.ResultatFile == null)
                    {
                        throw new Exception("Veuillez remplir les cases vides");
                    }

                    ImportCsvResult<CsvEtape> importEtapeCsvResult = await new CsvEtape().DispatchToTableAsync(_psqlContext, _hostEnvironment, _csvFolder, viewModel.EtapesFile);
                    ImportCsvResult<CsvResultat> importResultatResult = await new CsvResultat().DispatchToTableAsync(_psqlContext, _hostEnvironment, _csvFolder, viewModel.ResultatFile);

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e.StackTrace);
                    transaction.Rollback();
                    ViewBag.ErrorMessage = e.Message;
                }
            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult ImportationPoints()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ImportationPoints(ImportationViewModel viewModel)
        {
            using (var transaction = _psqlContext.Database.BeginTransaction())
            {
                try
                {
                    if (viewModel.PointsFile == null)
                    {
                        throw new Exception("Veuillez remplir les cases vides");
                    }

                    ImportCsvResult<CsvPoints> importPointsResult = await new CsvPoints().DispatchToTableAsync(_psqlContext, _hostEnvironment, _csvFolder, viewModel.PointsFile);
                    
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e.StackTrace);
                    transaction.Rollback();
                    ViewBag.ErrorMessage = e.Message;
                }
            }
            return View();
        }
    }
}
