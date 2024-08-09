using Evaluation.Models.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp.Media;
using System.Text;
using System.Text.RegularExpressions;

namespace Evaluation.Controllers
{
    [AllowAnonymous]
    public class UtilsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LoginModel()
        {
            return View();
        }

        public IActionResult AllForms()
        {
            return View();
        }

        public IActionResult Tableau()
        {
            return View();
        }

        public IActionResult TableauDeBord()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Histogramme(string anneestring)
        {
            int annee = int.Parse(anneestring);

            try
            {
                /*
                    List<MontantParMois> montantsParMois = new List<MontantParMois>();
                    for(int mois = 1; mois<=12; mois++)
                    {
                        var montant = _psqlContext.Devis
                            .Where(d => d.DateCreation.Year == annee && d.DateCreation.Month == mois)
                            .Sum(d => (double?)d.MontantTotal) ?? 0;

                        montantsParMois.Add(new MontantParMois
                        {
                            Mois = new DateTime(annee, mois, 1).ToString("MMMM"),
                            TotalMontant = montant
                        });
                    }
                    
                    return Json(new {resut = true, data = montantsParMois});
                 */

                return Json("success");
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("ERROR: "+e.Message);
                return Json(new { result = false, error = e.Message });
            }
        }

        [HttpGet]
        public IActionResult ExportExample()
        {
            return View();
        }

        public record RequestData(string htmlContent, string fileName);

        [HttpPost]
        public async Task<FileResult> ExportPdf([FromBody] RequestData requestData)
        {
            Stream pdfStream = await Export.ExportPdfAsync(requestData.htmlContent);

            return File(pdfStream, "application/pdf", requestData.fileName + ".pdf");
        }

        [HttpPost]
        public async Task<FileResult> ExportPdfA3([FromBody] RequestData requestData)
        {
            Stream pdfStream = await Export.ExportPdfAsync(requestData.htmlContent, PaperFormat.A3);

            return File(pdfStream, "application/pdf", requestData.fileName + ".pdf");
        }

        [HttpPost]
        public async Task<FileResult> ExportPdfA4([FromBody] RequestData requestData)
        {
            Stream pdfStream = await Export.ExportPdfAsync(requestData.htmlContent, PaperFormat.A4);

            return File(pdfStream, "application/pdf", requestData.fileName + ".pdf");
        }

        [HttpPost]
        public FileResult ExportExcel([FromBody] RequestData requestData)
        {
            return File(Encoding.UTF8.GetBytes(requestData.htmlContent), "application/vnd.ms-excel", requestData.fileName + ".xls");
        }

        [HttpGet]
        public IActionResult Certificat()
        {
            return View();
        }
    }
}
