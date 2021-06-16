namespace Dotnet6.EFCore6.Record.ValueObject.WebAPI.Models.Addresses
{
    public record StreetModel
    {
        public CityModel CityModel { get; init; }
        public string Name { get; init; }
        public long Number { get; init; }
    }
}