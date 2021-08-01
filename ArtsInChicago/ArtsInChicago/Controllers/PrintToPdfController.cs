using ArtsInChicago.Helpers;
using ArtsInChicago.Models;
using ArtsInChicago.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtsInChicago.Controllers
{
    public class PrintToPdfController : Controller
    {
        private readonly IPdfPrinter pdfPrinter;

        public PrintToPdfController(IPdfPrinter pdfPrinter)
        {
            this.pdfPrinter = pdfPrinter;
        }

        [HttpPost]
        public async Task<IActionResult> Print([FromBody] ArtworkDataFull model)
        {

            string fileName = await this.pdfPrinter.PrintIndividualArtwork(model);
            this.pdfPrinter.OpenDoc(fileName);

            return StatusCode(200);
        }

        public class Model
        {
            public string Artist { get; set; }
        }
    }
}
