using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtsInChicago.Models
{
    public class ArtworkType
    {
        [JsonProperty("title")]
        public string Title { get; set; }
    }
}
