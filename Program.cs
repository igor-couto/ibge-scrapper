using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using IBGE_Scrapper.Json;
using IBGE_Scrapper.Domain.Model;
using IBGE_Scrapper.Infrastructure;

namespace IBGE_Scrapper
{
    class Program
    {
        static async Task Main()
        {
            var UrlMunicipios = "https://servicodados.ibge.gov.br/api/v1/localidades/municipios";
            var UrlDistritos = "https://servicodados.ibge.gov.br/api/v1/localidades/municipios/{}/distritos";
            var connectionString = "mongodb://localhost:27017";

            var database = new Database(connectionString);

            var httpClient = new HttpClient() { 
                MaxResponseContentBufferSize = 100_000_000
            };

            var response = await httpClient.GetAsync(UrlMunicipios);

            if (response == null) return;
            
            var jsonString = await response.Content.ReadAsStringAsync();
            var citiesIbge = JsonConvert.DeserializeObject<IEnumerable<Municipio>>(jsonString);

            var cities = new List<City>();
            var microregions = new Dictionary<int, Microregion>();
            var mesoregions = new Dictionary<int, Mesoregion>();
            var states = new Dictionary<int, State>();
            var regions = new Dictionary<int, Region>();

            foreach (var city in citiesIbge)
            {
                var newCity = new City { 
                    Id = city.Id, 
                    Name = city.Nome, 
                    Mesoregion = city.Microrregiao.Mesorregiao.Nome, 
                    Microregion = city.Microrregiao.Nome,
                    State = city.Microrregiao.Mesorregiao.Uf.Nome,
                    Region = city.Microrregiao.Mesorregiao.Uf.Regiao.Nome
                };

                var responseDistricts = await httpClient.GetAsync(UrlDistritos.Replace("{}", newCity.Id.ToString()));
                var stringDistricts = await response.Content.ReadAsStringAsync();
                var districtIbge = JsonConvert.DeserializeObject<JsonArrayAttribute>(jsonString);

                cities.Add(newCity);

                if (!microregions.ContainsKey(city.Microrregiao.Id)) 
                {
                    var newMicroregion = new Microregion { 
                        Id = city.Microrregiao.Id, 
                        Name = city.Microrregiao.Nome,
                        Mesoregion = city.Microrregiao.Mesorregiao.Nome,
                        State = city.Microrregiao.Mesorregiao.Uf.Nome,
                        Region = city.Microrregiao.Mesorregiao.Uf.Regiao.Nome,
                    };
                    microregions.Add(newMicroregion.Id, newMicroregion);
                }

                if (!mesoregions.ContainsKey(city.Microrregiao.Mesorregiao.Id))
                {
                    var newMesoregion = new Mesoregion { 
                        Id = city.Microrregiao.Mesorregiao.Id, 
                        Name = city.Microrregiao.Mesorregiao.Nome,
                        State = city.Microrregiao.Mesorregiao.Uf.Nome,
                        Region = city.Microrregiao.Mesorregiao.Uf.Regiao.Nome,
                    };
                    mesoregions.Add(newMesoregion.Id, newMesoregion);
                }

                if (!states.ContainsKey(city.Microrregiao.Mesorregiao.Uf.Id))
                {
                    var newState = new State {
                        Id = city.Microrregiao.Mesorregiao.Uf.Id,
                        Name = city.Microrregiao.Mesorregiao.Uf.Nome,
                        Region = city.Microrregiao.Mesorregiao.Uf.Regiao.Nome,
                    };
                    states.Add(newState.Id, newState);
                }

                if (!regions.ContainsKey(city.Microrregiao.Mesorregiao.Uf.Regiao.Id))
                {
                    var newRegion = new Region
                    {
                        Id = city.Microrregiao.Mesorregiao.Uf.Regiao.Id,
                        Name = city.Microrregiao.Mesorregiao.Uf.Regiao.Nome,
                        Abbreviation = city.Microrregiao.Mesorregiao.Uf.Regiao.Sigla
                    };
                    regions.Add(newRegion.Id, newRegion);
                }
            }

            //foreach (var state in states) 
            //    await database.UpsertStateAsync(state.Value);

            //foreach (var mesoregion in mesoregions)
            //    await database.UpsertMesoregionAsync(mesoregion.Value);

            //foreach (var microregion in microregions)
            //    await database.UpsertMicroregionAsync(microregion.Value);

            //foreach (var region in regions)
            //    await database.UpsertRegionAsync(region.Value);

            await database.InsertCitiesAsync(cities);
        }
    }
}
