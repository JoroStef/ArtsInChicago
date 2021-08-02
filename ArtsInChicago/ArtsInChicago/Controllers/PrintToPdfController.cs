using ArtsInChicago.Helpers;
using ArtsInChicago.Models;
using ArtsInChicago.Services.Cotracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ArtsInChicago.Controllers
{
    public class PrintToPdfController : Controller
    {
        private readonly IPdfPrintService pdfPrinter;
        private readonly IWebHostEnvironment environment;

        public PrintToPdfController(IPdfPrintService pdfPrinter, IWebHostEnvironment environment)
        {
            this.pdfPrinter = pdfPrinter;
            this.environment = environment;
        }

        [HttpPost]
        public async Task<IActionResult> Print([FromBody] ArtworkDataFull model)
        {

            string imageString = await PdfPrintHelper.GetImageBase64String(model.ImageUrl, this.environment);

            string fileName = this.pdfPrinter.PrintIndividualArtwork(model, imageString);
            this.pdfPrinter.OpenDoc(fileName);

            return StatusCode(200);
        }

        public class Model
        {
            public string Artist { get; set; }
        }
    }
}
