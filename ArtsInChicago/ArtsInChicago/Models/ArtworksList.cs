using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtsInChicago.Models
{
    public class ArtworksList
    {
        public int PageNr { get; set; }

        [JsonProperty("data")]
        public List<Artwork> Data { get; set; }
    }
}
