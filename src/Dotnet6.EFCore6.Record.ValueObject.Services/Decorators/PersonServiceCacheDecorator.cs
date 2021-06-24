using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Dotnet6.EFCore6.Record.ValueObject.Domain.Entities;
using Dotnet6.EFCore6.Record.ValueObject.Repositories;
using Dotnet6.EFCore6.Record.ValueObject.Repositories.UnitsOfWork;
using Dotnet6.EFCore6.Record.ValueObject.Services.Abstractions;
using Dotnet6.EFCore6.Record.ValueObject.Services.Models;
using Dotnet6.EFCore6.Record.ValueObject.Services.Models.Addresses;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Caching.Memory;

namespace Dotnet6.EFCore6.Record.ValueObject.Services.Decorators
{
    public class PersonServiceCacheDecorator: Service<Person, PersonModel, Guid>, IPersonService
    {
        private readonly IMemoryCache _cache;
        private readonly IPersonService _service;

        public PersonServiceCacheDecorator(IPersonService service, IMemoryCache memoryCache, IPersonRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
            : base(repository, unitOfWork, mapper) => (_service, _cache) = (service, memoryCache);

        public Task DefineAddressAsync(Guid personId, AddressModel model, CancellationToken cancellationToken) 
            => _service.DefineAddressAsync(personId, model, cancellationToken);

        public override async Task<List<Person>> GetAllAsync(CancellationToken cancellationToken, Expression<Func<Person, bool>> predicate = default, Func<IQueryable<Person>, IOrderedQueryable<Person>> orderBy = default, Func<IQueryable<Person>, IIncludableQueryable<Person, object>> include = default, bool asTracking = default)
            => await _cache.GetOrCreateAsync(
                key: "AllPersons",
                factory: entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10);
                    return base.GetAllAsync(cancellationToken, predicate, orderBy, include, asTracking);
                });
    }
}