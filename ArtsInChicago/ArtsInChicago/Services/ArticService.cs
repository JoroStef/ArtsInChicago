﻿using ArtsInChicago.Models;
using ArtsInChicago.Services.Cotracts;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace ArtsInChicago.Services
{
    public class ArticService : IArticService
    {
        private readonly IConfiguration configuration;

        public ArticService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<ArtworksList> GetArtworksAsync(int? pageNr)
        {
            string endpoint = GetArtworksEndpoint(pageNr);

            var client = new HttpClient();

            using (var resource = await client.GetAsync(endpoint))
            {
                var result = await resource.Content.ReadAsStringAsync();

                var artworksList = JsonConvert.DeserializeObject<ArtworksList>(result);

                foreach (var item in artworksList.Data)
                {
                    string imageEndpoint = GetImageEndpoint(artworksList.ArticConfig.IIIFurl, item.ImageId);

                    //await GetImageAsync(imageEndpoint);
                    item.ImageUrl = imageEndpoint;
                }

                return artworksList;
            }
        }

        private async Task GetImageAsync(string endpoint)
        {
            var client = new HttpClient();

            using (var resource = await client.GetAsync(endpoint))
            {
                var result = await resource.Content.ReadAsStringAsync();

                //var foo = 123;
            }
        }

        private string GetArtworksEndpoint(int? pageNr)
        {
            if (pageNr == null || pageNr < 1)
            {
                pageNr = 1;
            }

            string basepoint = this.configuration["APIendpoints:BaseEndpoint"];
            string endpoint = basepoint + $"artworks?fields=id,title,artist_display,date_display,place_of_origin,image_id,main_reference_number&page={pageNr}";

            return endpoint;
        }

        private string GetImageEndpoint(string iifUrl, string imageId)
        {
            string endpoint = iifUrl + "/" + imageId + "/full/843,/0/default.jpg";

            return endpoint;
        }
    }
}