using IBGE_Scrapper.Domain.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IBGE_Scrapper.Infrastructure
{
    public class Database
    {
        private readonly IMongoDatabase _database;

        public Database(string connectionString) 
        {
            _database = new MongoClient(connectionString).GetDatabase("citiesbr");
        }

        public async Task UpsertCityAsync(City city) 
        {
            var collection = _database.GetCollection<City>("cities");

            var filter = new BsonDocument
            {
                { "Name", city.Name },
                { "State", city.State }
            };

            await collection.ReplaceOneAsync(
                    filter: filter,
                    options: new ReplaceOptions { IsUpsert = true },
                    replacement: city
                );
        }

        public async Task InsertCitiesAsync(List<City> cities)
        {
            var collection = _database.GetCollection<City>("cities");

            await collection.InsertManyAsync(cities);
        }

        public async Task UpsertMicroregionAsync(Microregion microregion)
        {
            var collection = _database.GetCollection<Microregion>("microregions");

            await collection.ReplaceOneAsync(
                    filter: new BsonDocument("Id", microregion.Id),
                    options: new ReplaceOptions { IsUpsert = true },
                    replacement: microregion
                );
        }

        public async Task UpsertMesoregionAsync(Mesoregion mesoregion)
        {
            var collection = _database.GetCollection<Mesoregion>("mesoregions");

            await collection.ReplaceOneAsync(
                    filter: new BsonDocument("Id", mesoregion.Id),
                    options: new ReplaceOptions { IsUpsert = true },
                    replacement: mesoregion
                );
        }

        public async Task UpsertStateAsync(State state)
        {
            var collection = _database.GetCollection<State>("states");

            await collection.ReplaceOneAsync(
                    filter: new BsonDocument("Name", state.Name),
                    options: new ReplaceOptions { IsUpsert = true },
                    replacement: state
                );
        }

        public async Task UpsertRegionAsync(Region region)
        {
            var collection = _database.GetCollection<Region>("regions");

            await collection.ReplaceOneAsync(
                    filter: new BsonDocument("Name", region.Name),
                    options: new ReplaceOptions { IsUpsert = true },
                    replacement: region
                );
        }
    }
}