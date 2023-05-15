using HMS.Service.Domain.Abstractions.Interfaces;

namespace HMS.Service.Data
{
    public class MongoConfigOptions : IMongoConfigOptions
    {
        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ServiceType { get; set; }
    }
    }
