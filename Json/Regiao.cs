﻿using Newtonsoft.Json;

namespace IBGE_Scrapper.Json
{
    public class Regiao
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("sigla")]
        public string Sigla { get; set; }
    }
}
