namespace Dotnet6.EFCore6.Record.ValueObject.WebAPI.Models.Addresses
{
    public record CountryModel
    {
        public string Initials { get; init; }
        public string Name { get; init; }
    }
}