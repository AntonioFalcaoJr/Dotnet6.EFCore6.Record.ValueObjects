namespace Dotnet6.EFCore6.Record.ValueObject.Domain.ValueObjects
{
    public record Address : Abstractions.ValueObject
    {
        // Empty constructor in this case is required by EF Core,
        // because has a complex type as a parameter in the default constructor.
        private Address() { }

        public Address(Street street, string zipCode)
            => (Street, ZipCode) = (street, zipCode);

        public Street Street { get; private init; }
        public string ZipCode { get; private init; }
    }
}