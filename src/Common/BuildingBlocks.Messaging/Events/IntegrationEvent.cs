namespace BuildingBlocks.Messaging.Events
{
    //Base class which provide common propertis to all others integration events
    public record IntegrationEvent
    {
        public Guid Id => Guid.NewGuid();

        public DateTime OccuredOn = DateTime.Now;

        public string EventType => GetType().AssemblyQualifiedName!;
    }
}
