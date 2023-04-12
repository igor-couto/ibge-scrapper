using Newtonsoft.Json;

namespace IBGE_Scrapper.Json
{
    public class Municipio
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("microrregiao")]
        public Microrregiao Microrregiao { get;  set; }
    }
}
