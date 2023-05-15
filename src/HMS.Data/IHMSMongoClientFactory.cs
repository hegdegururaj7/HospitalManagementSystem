using MongoDB.Driver;

namespace HMS.Service.Data
{
    public interface IHMSMongoClientFactory
    {
        MongoClient Create();
    }
}
