namespace Dotnet6.EFCore6.Record.ValueObject.WebAPI.Models.Addresses
{
    public record CityModel
    {
        public string Name { get; init; }
        public StateModel StateModel { get; init; }
    }
}