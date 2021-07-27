using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtsInChicago.Models
{
    public class ArticConfig
    {
        [JsonProperty("iiif_url")]
        public string IIIFurl { get; set; }
    }
}
