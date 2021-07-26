using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArtsInChicago.Models
{
    public class Artwork
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("artist_display")]
        public string Artist { get; set; }

        [JsonProperty("date_display")]
        public string Date { get; set; }

        [JsonProperty("main_reference_number")]
        [DisplayName("Ref. Number")]
        public string RefNumber { get; set; }

    }
}
