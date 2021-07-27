using ArtsInChicago.Services.Cotracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtsInChicago.Controllers
{
    public class GalleryController : Controller
    {
        private readonly IArticService articService;

        public GalleryController(IArticService articService)
        {
            this.articService = articService;
        }

        public IActionResult Index(int? pageNumber)
        {
            try
            {
                var artworksList = await this.artworksListService.GetArtworksAsync(pageNumber);

                return View(artworksList);

            }
            catch (Exception)
            {

                return RedirectToAction("Error", "Home");
            }
        }
    }
}
