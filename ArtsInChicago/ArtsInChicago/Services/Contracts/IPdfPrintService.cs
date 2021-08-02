using ArtsInChicago.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtsInChicago.Services.Cotracts
{
    public interface IPdfPrintService
    {
        string PrintIndividualArtwork(ArtworkDataFull model, string imageString);

        void OpenDoc(string fileName);
    }
}
