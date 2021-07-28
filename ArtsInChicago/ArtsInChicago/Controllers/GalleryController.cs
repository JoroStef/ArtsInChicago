using ArtsInChicago.Helpers;
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
        private const string PAGE = "currentGalleryPage";
        private const string PAGE_NUMBER = "galleryPageNumber";

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

                CachHelper.CachInMemory(artworksList, PAGE, this.memoryCache);
                CachHelper.CachInMemory(artworksList.PagingParams.CurrentPage, PAGE_NUMBER, this.memoryCache, 60);

                return View(artworksList);

            }
            catch (Exception)
            {

                return RedirectToAction("Error", "Home");
            }
        }

        public IActionResult IndexCached()
        {
            var cacheOutput = CachHelper.GetCachedInMemory<ArtworksList>(this.memoryCache, PAGE, PAGE_NUMBER);

            if (cacheOutput.obj!=null)
            {
                return View("Index", cacheOutput.obj);
            }
            else
            {
                return RedirectToAction("Index", routeValues: new { pageNumber = cacheOutput.pageNumber });
            }
        }
    }
}
