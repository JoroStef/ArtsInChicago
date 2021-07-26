using ArtsInChicago.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArtsInChicago.Services.Cotracts
{
    public interface IArtworksListService
    {
        Task<ArtworksList> GetArtworksAsync(int? pageNr);

    }
}
