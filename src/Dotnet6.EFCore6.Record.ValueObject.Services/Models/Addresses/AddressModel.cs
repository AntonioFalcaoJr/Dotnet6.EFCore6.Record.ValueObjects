namespace Dotnet6.EFCore6.Record.ValueObject.Services.Models.Addresses
{
    public record AddressModel
    {
        public StreetModel StreetModel { get; init; }
        public string ZipCode { get; init; }
    }
}