namespace HMS.Service.Domain.Abstractions.Interfaces
{
    public interface IMongoConfigOptions
    {
        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ServiceType { get; set; }
    }
}
