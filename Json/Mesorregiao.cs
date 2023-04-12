using Newtonsoft.Json;

namespace IBGE_Scrapper.Json
{
    public class Mesorregiao
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("UF")]
        public Uf Uf { get; set; }
    }
}
