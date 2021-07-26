using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtsInChicago.Controllers
{
    public class ArtworksListController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
