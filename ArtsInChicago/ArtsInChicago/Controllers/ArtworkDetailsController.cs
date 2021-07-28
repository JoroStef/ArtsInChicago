using ArtsInChicago.Models;
using ArtsInChicago.Models.ViewModels;
using ArtsInChicago.Services.Cotracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtsInChicago.Controllers
{
    public class ArtworkDetailsController : Controller
    {
        private readonly IArticService articService;

        public ArtworkDetailsController(IArticService articService)
        {
            this.articService = articService;
        }

        public async Task<IActionResult> Index(int id, string backController)
        {
            try
            {
                var artwork = await this.articService.GetArtworkByIdAsync(id);

                //return View(artwork);
                return View(new DetailsViewModel() { BackController = backController, Details = artwork });

            }
            catch(ArgumentNullException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

    }
}
