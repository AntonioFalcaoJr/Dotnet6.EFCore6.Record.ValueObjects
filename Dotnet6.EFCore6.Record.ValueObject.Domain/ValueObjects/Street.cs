namespace Dotnet6.EFCore6.Record.ValueObject.Domain.ValueObjects
{
    public record Street : Abstractions.ValueObject
    {
        private Street() { }

        public Street(City city, string name, int number) 
            => (City, Name, Number) = (city, name, number);

        public City City { get; private init; }
        public string Name { get; private init;}
        public int Number { get; private init;}
    }
}