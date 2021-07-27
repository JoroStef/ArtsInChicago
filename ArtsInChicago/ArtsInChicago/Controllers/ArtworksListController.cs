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
        private readonly IArtworksListService artworksListService;

        public ArtworksListController(IArtworksListService artworksListService)
        {
            this.artworksListService = artworksListService;
        }

        public async Task<IActionResult> Index(int? pageNumber)
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
