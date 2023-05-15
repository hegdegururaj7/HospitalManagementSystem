using HMS.Service.Domain.Abstractions.Entities;
using HMS.Service.Domain.Abstractions.Interfaces;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;
using MongoDB.Driver.Core.Extensions.DiagnosticSources;
using System.Diagnostics;
using System.Net;

namespace HMS.Service.Data
{
    public class HMSMongoClientFactory : IHMSMongoClientFactory
    {
        private readonly IMongoConfigOptions _configOptions;
        private MongoClient _client;


        private static readonly object _lock = new();

        public HMSMongoClientFactory(IMongoConfigOptions configOptions)
        {
            _configOptions = configOptions;
        }

        public MongoClient Create()
        {
            if (_client != null)
                return _client;
            // BsonClassMap.IsClassMapRegistered is not threadsafe,
            // locking here will prevent threads from causing ArgumentException errors
            lock (_lock)
            {
                ConventionRegistry.Register("Ignore Nulls", new ConventionPack
                {
                    new IgnoreIfNullConvention(true)
                },
                t => true);

                registerMapCustomClass();

              
                    var mongoConnectionString = string.IsNullOrWhiteSpace("root")
                        ? $"mongodb://{_configOptions.Host}"
                        : $"mongodb://root:{WebUtility.UrlEncode("P@55W0RD")}@{"localhost:27017"}";
                    var settings = MongoClientSettings.FromConnectionString(mongoConnectionString);
                   settings.SslSettings = new SslSettings()
                {
                    EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12
                };
                settings.MaxConnectionLifeTime = TimeSpan.FromHours(24);
                settings.SocketTimeout = TimeSpan.FromMinutes(15);
                settings.MaxConnectionIdleTime = TimeSpan.FromMinutes(20);
                settings.ConnectTimeout = TimeSpan.FromMinutes(5);
                settings.ServerSelectionTimeout = TimeSpan.FromMinutes(5);
                settings.MinConnectionPoolSize = 10;
                settings.MaxConnectionPoolSize = 100;
                settings.ClusterConfigurator = builder =>
                    {
                        builder.Subscribe<CommandStartedEvent>(s => Debugger.Log(1 /* Debug */,
                            "MongoDb", s.Command.ToString()));
                        builder.Subscribe(new DiagnosticsActivityEventSubscriber());
                    };
                    _client = new MongoClient(settings);
                return _client == null ? new MongoClient() : _client;

            }
        }

        private static void registerMapCustomClass()
        {
            HMSMapRegistrations();
        }

        private static void HMSMapRegistrations()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(PatientEntity)))
            {
                BsonClassMap.RegisterClassMap<PatientEntity>(map =>
                {
                    map.AutoMap();
                    map.SetIgnoreExtraElements(true);
                });
            }
        }
    }
}


