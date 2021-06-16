using AutoMapper;
using Dotnet6.EFCore6.Record.ValueObject.Domain.Entities;
using Dotnet6.EFCore6.Record.ValueObject.Domain.ValueObjects;
using Dotnet6.EFCore6.Record.ValueObject.WebAPI.Models;
using Dotnet6.EFCore6.Record.ValueObject.WebAPI.Models.Addresses;

namespace Dotnet6.EFCore6.Record.ValueObject.WebAPI.MappingProfiles
{
    public class ModelToDomainProfile : Profile
    {
        public ModelToDomainProfile()
        {
            CreateMap<PersonModel, Person>();

            CreateMap<AddressModel, Address>()
                .ForPath(dest => dest.Street, conf => conf.MapFrom(src => src.StreetModel));

            CreateMap<StreetModel, Street>()
                .ForPath(dest => dest.City, conf => conf.MapFrom(src => src.CityModel));

            CreateMap<CityModel, City>()
                .ForPath(dest => dest.State, conf => conf.MapFrom(src => src.StateModel));

            CreateMap<StateModel, State>()
                .ForPath(dest => dest.Country, conf => conf.MapFrom(src => src.CountryModel));

            CreateMap<CountryModel, Country>();
        }
    }
}