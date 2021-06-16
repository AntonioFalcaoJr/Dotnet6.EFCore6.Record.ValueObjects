namespace Dotnet6.EFCore6.Record.ValueObject.Domain.Entities.Abstractions
{
    public class Entity<TId>
        where TId : struct
    {
        public TId Id { get; init; }
    }
}