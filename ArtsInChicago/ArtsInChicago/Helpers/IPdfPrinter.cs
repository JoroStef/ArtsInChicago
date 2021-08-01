using ArtsInChicago.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtsInChicago.Helpers
{
    public interface IPdfPrinter
    {
        Task<string> PrintIndividualArtwork(ArtworkDataFull model);

        void OpenDoc(string fileName);
    }
}
