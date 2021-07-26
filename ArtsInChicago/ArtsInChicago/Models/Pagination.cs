using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtsInChicago.Models
{
    public class Pagination
    {
        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }

        [JsonProperty("total")]
        public int TotalReccords { get; set; }

        [JsonProperty("current_page")]
        public int CurrentPage { get; set; }

    }
}
