namespace Dotnet6.EFCore6.Record.ValueObject.Domain.ValueObjects
{
    public record Country : Abstractions.ValueObject
    {
        private Country() { }

        public Country(string initials, string name)
            => (Initials, Name) = (initials, name);

        public string Initials { get; private init; }
        public string Name { get; private init; }
    }
}