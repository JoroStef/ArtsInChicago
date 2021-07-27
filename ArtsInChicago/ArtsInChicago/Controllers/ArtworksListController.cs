using ArtsInChicago.Services.Cotracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtsInChicago.Controllers
{
    public class ArtworksListController : Controller
    {
        private readonly IArticService articService;

        public ArtworksListController(IArticService articService)
        {
            this.articService = articService;
        }

        public async Task<IActionResult> Index(int? pageNumber)
        {
            try
            {
                var artworksList = await this.articService.GetArtworksAsync(pageNumber);

                return View(artworksList);

            }
            catch (Exception)
            {

                return RedirectToAction("Error", "Home");
            }        
        }
    }
}
