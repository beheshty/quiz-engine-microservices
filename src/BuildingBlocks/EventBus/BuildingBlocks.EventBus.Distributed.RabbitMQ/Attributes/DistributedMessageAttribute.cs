namespace BuildingBlocks.EventBus.Distributed.RabbitMQ.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class DistributedMessageAttribute : Attribute
    {
        public string ExchangeName { get; set; } = "event_bus";
        public ExchangeType ExchangeType { get; set; } = ExchangeType.Fanout;
        public string QueueName { get; set; }

        public bool Durable { get; set; } = true;

        public DistributedMessageAttribute(string queueName)
        {
            QueueName = queueName;
        }
    }
}
