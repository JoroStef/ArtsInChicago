using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtsInChicago.Models
{
    public class ArtworksList
    {
        [JsonProperty("pagination")]
        public Pagination PagingParams { get; set; }

        [JsonProperty("data")]
        public List<Artwork> Data { get; set; }

        [JsonProperty("config")]
        public ArticConfig ArticConfig { get; set; }
    }
}
