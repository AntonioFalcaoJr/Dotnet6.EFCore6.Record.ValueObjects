using System;
using System.Threading;
using System.Threading.Tasks;
using Dotnet6.EFCore6.Record.ValueObject.Domain.Entities;
using Dotnet6.EFCore6.Record.ValueObject.Services.Abstractions;
using Dotnet6.EFCore6.Record.ValueObject.Services.Models;
using Dotnet6.EFCore6.Record.ValueObject.Services.Models.Addresses;

namespace Dotnet6.EFCore6.Record.ValueObject.Services
{
    public interface IPersonService : IService<Person, PersonModel, Guid>
    {
        Task DefineAddressAsync(Guid personId, AddressModel model, CancellationToken cancellationToken);
    }
}