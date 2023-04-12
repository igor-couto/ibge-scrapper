using Newtonsoft.Json;

namespace IBGE_Scrapper.Json
{
    public class Uf
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("sigla")]
        public string Sigla { get; set; }

        [JsonProperty("regiao")]
        public Regiao Regiao { get; set; }
    }
}
