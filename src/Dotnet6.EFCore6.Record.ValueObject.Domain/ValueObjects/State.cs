namespace Dotnet6.EFCore6.Record.ValueObject.Domain.ValueObjects
{
    public record State : Abstractions.ValueObject
    {
        private State() { }

        public State(Country country, string initials, string name)
            => (Country, Initials, Name) = (country, initials, name);

        public Country Country { get; private init; }
        public string Initials { get; }
        public string Name { get; }
    }
}