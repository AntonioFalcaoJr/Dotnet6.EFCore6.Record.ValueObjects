namespace Dotnet6.EFCore6.Record.ValueObject.Domain.ValueObjects
{
    public sealed record City : Abstractions.ValueObject
    {
        private City() { }

        public City(string name, State state)
            => (Name, State) = (name, state);

        public string Name { get; }
        public State State { get; private init; }
    }
}