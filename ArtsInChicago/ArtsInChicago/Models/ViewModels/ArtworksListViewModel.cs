using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtsInChicago.Models.ViewModels
{
    public class ArtworksListViewModel
    {
        public ArtworksList ArtworksList { get; set; }

        public IEnumerable<string> ArtworkTypes { get; set; }
    }
}
