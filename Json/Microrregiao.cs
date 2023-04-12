using Newtonsoft.Json;

namespace IBGE_Scrapper.Json
{
    public class Microrregiao
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("mesorregiao")]
        public Mesorregiao Mesorregiao { get; set; }
    }
}
