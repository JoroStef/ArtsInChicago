using Newtonsoft.Json;
using System.Collections.Generic;


namespace ArtsInChicago.Models
{
    public class Description
    {

        [JsonProperty("description")]
        public List<DescriptionItems> Items { get; set; }

    }

}