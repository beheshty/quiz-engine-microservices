namespace BuildingBlocks.EventBus.Distributed.RabbitMQ.Configuration
{
    public class RabbitMqOptions
    {
        public string HostUrl { get; set; } = string.Empty;
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
