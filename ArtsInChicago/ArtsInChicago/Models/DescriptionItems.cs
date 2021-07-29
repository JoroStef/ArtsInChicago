using Newtonsoft.Json;


namespace ArtsInChicago.Models
{
    public class DescriptionItems
    {
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

    }

}