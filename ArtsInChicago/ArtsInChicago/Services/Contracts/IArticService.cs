using ArtsInChicago.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArtsInChicago.Services.Cotracts
{
    public interface IArticService
    {
        Task<ArtworksList> GetArtworksAsync(int? pageNr, int? pageLimit, string artworkType);

        Task<ArtworkDetails> GetArtworkByIdAsync(int id);
    }
}
