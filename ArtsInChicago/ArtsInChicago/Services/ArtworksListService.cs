using ArtsInChicago.Models;
using ArtsInChicago.Services.Cotracts;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace ArtsInChicago.Services
{
    public class ArtworksListService : IArtworksListService
    {
        private readonly IConfiguration configuration;

        public ArtworksListService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<ArtworksList> GetArtworksAsync(int? pageNr)
        {
            if (pageNr == null || pageNr < 1)
            {
                pageNr = 1;
            }

            var client = new HttpClient();

            string basepoint = this.configuration["APIendpoints:BaseEndpoint"];
            string endpoint = basepoint + $"artworks?fields=id,title,artist_display,date_display,main_reference_number&page={pageNr}";

            using (var resource = await client.GetAsync(endpoint))
            {
                var result = await resource.Content.ReadAsStringAsync();

                var artworksList = JsonConvert.DeserializeObject<ArtworksList>(result);

                //artworksList.PageNr = pageNr.Value;

                return artworksList;
            }
        }
    }
}
