using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtsInChicago.Models
{
    public class ArtworkDataFull : ArtworkDataPartial
    {
        [JsonProperty("medium_display")]
        public string Medium { get; set; }

        [JsonProperty("dimensions")]
        public string Dimentions { get; set; }

        public string Description { get; set; }
    }
}
