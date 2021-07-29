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
        [HttpPost]
        public IActionResult Print([FromBody] ArtworkDataFull model)
        {
            PdfPrinter.PrintIndividualArtwork(model, out string fileName);
            PdfPrinter.OpenDoc(fileName);

            return View();
        }

        public class Model
        {
            public string Artist { get; set; }
        }
    }
}
