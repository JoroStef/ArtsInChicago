using ArtsInChicago.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtsInChicago.Helpers
{
    public class PdfPrinterMigradoc : IPdfPrinter
    {
        public void OpenDoc(string fileName)
        {
            throw new NotImplementedException();
        }

        public Task<string> PrintIndividualArtwork(ArtworkDataFull model)
        {
            throw new NotImplementedException();
        }
    }
}
