namespace BuildingBlocks.EventBus.Domain
{
    public class DomainEventBusOptions
    {
        public bool UseLocal { get; set; }
        public bool UseDistributed { get; set; }
    }
}