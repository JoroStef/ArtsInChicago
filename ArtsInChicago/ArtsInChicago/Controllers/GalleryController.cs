using ArtsInChicago.Models;
using ArtsInChicago.Services.Cotracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace ArtsInChicago.Controllers
{
    public class GalleryController : Controller
    {
        private readonly IArticService articService;
        private readonly IMemoryCache memoryCache;

        public GalleryController(IArticService articService, IMemoryCache memoryCache)
        {
            this.articService = articService;
            this.memoryCache = memoryCache;
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
