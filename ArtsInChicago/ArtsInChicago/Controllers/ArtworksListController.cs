using ArtsInChicago.Helpers;
using ArtsInChicago.Models;
using ArtsInChicago.Models.ViewModels;
using ArtsInChicago.Services.Contracts;
using ArtsInChicago.Services.Cotracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtsInChicago.Controllers
{
    public class ArtworksListController : Controller
    {
        private const string PAGE = "currentListPage";
        private const string PAGE_NUMBER = "listPageNumber";

        private readonly IArticService articService;
        private readonly IArtworkTypesService artworkTypesService;
        private readonly IMemoryCache memoryCache;

        public ArtworksListController(IArticService articService, IArtworkTypesService artworkTypesService, IMemoryCache memoryCache)
        {
            this.articService = articService;
            this.artworkTypesService = artworkTypesService;
            this.memoryCache = memoryCache;
        }

        public async Task<IActionResult> Index(int? pageNumber, string artwork_type)
        {
            try
            {
                var typesCount = await this.artworkTypesService.GetCountAsync();
                var typesCollection = await this.artworkTypesService.GetAllAsync(typesCount);

                var artworksList = await this.articService.GetArtworksAsync(pageNumber, null, artwork_type);

                CachHelper.CachInMemory(artworksList, PAGE, this.memoryCache);
                CachHelper.CachInMemory(artworksList.PagingParams.CurrentPage, PAGE_NUMBER, this.memoryCache, 60);

                var viewModel = new ArtworksListViewModel();
                viewModel.ArtworksList = artworksList;
                viewModel.ArtworkTypes = typesCollection;

                return View(viewModel);

            }
            catch (Exception)
            {

                return RedirectToAction("Error", "Home");
            }        
        }

        public IActionResult IndexCached()
        {
            var cacheOutput = CachHelper.GetCachedInMemory<ArtworksList>(this.memoryCache, PAGE, PAGE_NUMBER);

            if (cacheOutput.obj != null)
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
