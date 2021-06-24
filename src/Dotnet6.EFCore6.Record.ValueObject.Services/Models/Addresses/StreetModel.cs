namespace Dotnet6.EFCore6.Record.ValueObject.Services.Models.Addresses
{
    public record StreetModel
    {
        public CityModel CityModel { get; init; }
        public string Name { get; init; }
        public long Number { get; init; }
    }
}