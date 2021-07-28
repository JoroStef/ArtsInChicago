using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtsInChicago.Models
{
    public class ArtworkDetails
    {
        [JsonProperty("data")]
        public ArtworkDataFull Data { get; set; }

        [JsonProperty("config")]
        public ArticConfig ArticConfig { get; set; }

    }
}
