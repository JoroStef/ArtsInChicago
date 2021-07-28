using ArtsInChicago.Services.Cotracts;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<IActionResult> Index(int id)
        {
            try
            {
                var artwork = await this.articService.GetArtworkByIdAsync(-1);

                return View(artwork);

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
