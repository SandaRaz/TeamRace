using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace Evaluation.Models.Utils
{
    public class Export
    {
        public static async Task<Stream> ExportPdfAsync(string htmlContent)
        {
            var launchOptions = new LaunchOptions
            {
                Headless = true,
                ExecutablePath = @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe"
            };

            await using var browser = await Puppeteer.LaunchAsync(launchOptions);
            await using var page = await browser.NewPageAsync();

            await page.SetContentAsync(htmlContent);

            //await Task.Delay(1000);

            var pdfOptions = new PdfOptions
            {
                Format = PaperFormat.A4,
                PrintBackground = true
            };

            var pdfStream = await page.PdfStreamAsync(pdfOptions);
            pdfStream.Position = 0;

            return pdfStream;
        }

        public static async Task<Stream> ExportPdfAsync(string htmlContent, PaperFormat paperFormat)
        {
            var launchOptions = new LaunchOptions
            {
                Headless = true,
                ExecutablePath = @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe"
            };

            await using var browser = await Puppeteer.LaunchAsync(launchOptions);
            await using var page = await browser.NewPageAsync();

            await page.SetContentAsync(htmlContent);

            //await Task.Delay(1000);

            var pdfOptions = new PdfOptions
            {
                Format = paperFormat,
                PrintBackground = true
            };

            var pdfStream = await page.PdfStreamAsync(pdfOptions);
            pdfStream.Position = 0;

            return pdfStream;
        }

    }
}
