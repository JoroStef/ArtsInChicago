using ArtsInChicago.Models;
using ArtsInChicago.Services.Cotracts;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System.Net;
using System;
using System.Text;

namespace ArtsInChicago.Services
{
    public class ArticService : IArticService
    {
        private readonly IConfiguration configuration;

        public ArticService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<ArtworkDetails> GetArtworkByIdAsync(int id)
        {
            string[] includeFields = { "id", "title", "artist_title", "date_display", "place_of_origin", "department_title", "image_id", "main_reference_number", "medium_display", "dimensions" };
            string endpoint = GetEndpoint(includeFields, routeParam: id.ToString());

            var client = new HttpClient();

            using (var resource = await client.GetAsync(endpoint))
            {
                if (resource.StatusCode != HttpStatusCode.OK)
                {
                    throw new ArgumentNullException(resource.ReasonPhrase);
                }

                var result = await resource.Content.ReadAsStringAsync();

                var artwork = JsonConvert.DeserializeObject<ArtworkDetails>(result);

                string imageEndpoint = GetImageEndpoint(artwork.ArticConfig.IIIFurl, artwork.Data.ImageId, 843);

                artwork.Data.ImageUrl = imageEndpoint;

                return artwork;
            }
        }

        public async Task<ArtworksList> GetArtworksAsync(int? pageNr)
        {
            if (pageNr == null || pageNr < 1)
            {
                pageNr = 1;
            }

            string[] includeFields = { "id", "title", "artist_title", "date_display", "place_of_origin", "department_title", "image_id", "main_reference_number" };
            string endpoint = GetEndpoint(includeFields, pageNr);

            var client = new HttpClient();

            using (var resource = await client.GetAsync(endpoint))
            {
                var result = await resource.Content.ReadAsStringAsync();

                var artworksList = JsonConvert.DeserializeObject<ArtworksList>(result);

                foreach (var item in artworksList.Data)
                {
                    string imageEndpoint = GetImageEndpoint(artworksList.ArticConfig.IIIFurl, item.ImageId, 400);

                    item.ImageUrl = imageEndpoint;
                }

                return artworksList;
            }
        }

        #region Not used
        //private async Task DownloadImageAsync(string endpoint, string fileName)
        //{
        //    using (WebClient webClient = new WebClient())
        //    {
        //        var imageUri = new Uri(endpoint);

        //        await webClient.DownloadFileTaskAsync(imageUri, fileName);

        //    }
        //}

        //private string GetArtworksEndpoint(int? pageNr)
        //{
        //    if (pageNr == null || pageNr < 1)
        //    {
        //        pageNr = 1;
        //    }

        //    string basepoint = this.configuration["APIendpoints:BaseEndpointArtworks"];
        //    string endpoint = basepoint +
        //        $"?fields=id,title,artist_title,date_display,place_of_origin,department_title,image_id,main_reference_number" +
        //        //$"&is_on_view=1" +
        //        $"&page={pageNr}";

        //    return endpoint;
        //}

        #endregion

        private string GetEndpoint(string[] includeFields, int? pageNr = null, string routeParam = "")
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(this.configuration["APIendpoints:BaseEndpointArtworks"]);

            if (!string.IsNullOrEmpty(routeParam))
            {
                sb.Append($"/{routeParam}");
            }

            if (includeFields.Length != 0 || pageNr != null)
            {
                sb.Append("?");
            }

            if (pageNr != null)
            {
                sb.Append($"page={pageNr}");
            }

            if (includeFields.Length != 0)
            {
                sb.Append($"&fields={string.Join(',', includeFields)}");
            }

            return sb.ToString().Trim();
        }

        private string GetImageEndpoint(string iifUrl, string imageId, int size)
        {
            string endpoint = $"{iifUrl}/{imageId}/full/{size},/0/default.jpg";

            return endpoint;
        }
    }
}
