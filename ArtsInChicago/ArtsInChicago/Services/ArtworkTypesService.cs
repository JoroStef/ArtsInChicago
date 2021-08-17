using ArtsInChicago.Models;
using ArtsInChicago.Services.Contracts;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ArtsInChicago.Services
{
    public class ArtworkTypesService : IArtworkTypesService
    {
        private readonly IConfiguration configuration;

        public ArtworkTypesService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<IEnumerable<string>> GetAllAsync(int? pageLimit)
        {
            string[] includeFields = { "title" };

            string endpoint = GetEndpoint(includeFields, pageNr: null, pageLimit);

            var client = new HttpClient();

            using (var resource = await client.GetAsync(endpoint))
            {
                var result = await resource.Content.ReadAsStringAsync();

                JObject resultJson = JObject.Parse(result);

                JArray typesJson = (JArray)resultJson.SelectToken("data");

                IEnumerable<string> types = typesJson.Select(t => (string)t.SelectToken("title"));
 
                return types;
            }
        }

        public async Task<int> GetCountAsync()
        {
            string[] includeFields = { "id" };

            string endpoint = GetEndpoint(includeFields, pageNr: null, pageLimit: 0);

            var client = new HttpClient();

            using (var resource = await client.GetAsync(endpoint))
            {
                var result = await resource.Content.ReadAsStringAsync();

                JObject resultJson = JObject.Parse(result);

                int count = (int)resultJson.SelectToken("pagination.total");

                return count;
            }

        }

        private string GetEndpoint(string[] includeFields, int? pageNr = null, int? pageLimit = null, string routeParam = "")
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(this.configuration["APIendpoints:BaseEndpointArtworkTypes"]);

            if (!string.IsNullOrEmpty(routeParam))
            {
                sb.Append($"/{routeParam}");
            }

            List<string> queryParams = new List<string>();

            if (pageNr != null)
            {
                queryParams.Add($"page={pageNr}");
            }

            if (pageLimit != null)
            {
                queryParams.Add($"limit={pageLimit}");
            }

            if (includeFields.Length != 0)
            {
                queryParams.Add($"fields={string.Join(',', includeFields)}");
            }

            if (queryParams.Count != 0)
            {
                sb.Append("?");
                sb.Append(string.Join('&', queryParams));
            }

            return sb.ToString().Trim();
        }

    }
}
