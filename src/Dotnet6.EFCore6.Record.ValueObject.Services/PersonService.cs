using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Dotnet6.EFCore6.Record.ValueObject.Domain.Entities;
using Dotnet6.EFCore6.Record.ValueObject.Domain.Exceptions;
using Dotnet6.EFCore6.Record.ValueObject.Domain.ValueObjects;
using Dotnet6.EFCore6.Record.ValueObject.Repositories;
using Dotnet6.EFCore6.Record.ValueObject.Repositories.UnitsOfWork;
using Dotnet6.EFCore6.Record.ValueObject.Services.Abstractions;
using Dotnet6.EFCore6.Record.ValueObject.Services.Models;
using Dotnet6.EFCore6.Record.ValueObject.Services.Models.Addresses;
using Microsoft.EntityFrameworkCore;

namespace Dotnet6.EFCore6.Record.ValueObject.Services
{
    public class PersonService : Service<Person, PersonModel, Guid>, IPersonService
    {
        public PersonService(IPersonRepository repository, IUnitOfWork unitOfWork, IMapper mapper) 
            : base(repository, unitOfWork, mapper) { }

        public async Task DefineAddressAsync(Guid personId, AddressModel model, CancellationToken cancellationToken)
        {
            var person = await Repository.GetByIdAsync(
                id: personId,
                include: persons => persons.Include(entity => entity.Address),
                cancellationToken: cancellationToken,
                asTracking: true);

            if (person is null) 
                throw new BusinessException("Person not found.");

            var address = Mapper.Map<AddressModel, Address>(model);
            person.DefineAddress(address);

            await UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}