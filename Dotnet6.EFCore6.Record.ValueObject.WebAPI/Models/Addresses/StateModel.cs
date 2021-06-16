namespace Dotnet6.EFCore6.Record.ValueObject.WebAPI.Models.Addresses
{
    public record StateModel
    {
        public CountryModel CountryModel { get; init; }
        public string Initials { get; init; }
        public string Name { get; init; }
    }
}