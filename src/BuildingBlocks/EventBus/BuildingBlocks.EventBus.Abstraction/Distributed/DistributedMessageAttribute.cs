namespace BuildingBlocks.EventBus.Distributed.RabbitMQ.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class DistributedMessageAttribute : Attribute
    {
        public string Destination { get; set; } = "event_bus";
        public string DistributionStrategy { get; set; } = "fanout";
        public string Subscription { get; set; }

        public DistributedMessageAttribute(string subscription)
        {
            Subscription = subscription;
        }
    }
}
