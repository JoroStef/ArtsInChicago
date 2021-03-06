using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArtsInChicago.Models
{
    public class ArtworkDataPartial
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("artist_title")]
        public string Artist { get; set; }

        [JsonProperty("date_display")]
        public string Date { get; set; }

        [JsonProperty("place_of_origin")]
        public string PlaceOfOrigin { get; set; }

        [JsonProperty("department_title")]
        public string Department { get; set; }

        [JsonProperty("image_id")]
        public string ImageId { get; set; }

        [JsonProperty("main_reference_number")]
        [DisplayName("Ref. Number")]
        public string RefNumber { get; set; }

        public string ImageUrl { get; set; }
    }
}
