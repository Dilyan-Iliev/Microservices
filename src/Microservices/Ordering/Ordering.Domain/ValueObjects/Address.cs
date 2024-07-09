namespace Ordering.Domain.ValueObjects
{
    //Value object - it is immutable (we will not have setters and will use record type)
    public record Address //related with custom address
    {
        public string FirstName { get; } = default!;
        public string LastName { get; } = default!;
        public string? EmailAddress { get; } = default!;
        public string AddressLine { get; } = default!;
        public string Country { get; } = default!;
        public string State { get; } = default!;
        public string ZipCode { get; } = default!;
    }
}
