using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Dotnet6.EFCore6.Record.ValueObject.Domain.Entities;
using Dotnet6.EFCore6.Record.ValueObject.Repositories;
using Dotnet6.EFCore6.Record.ValueObject.Repositories.UnitsOfWork;
using Dotnet6.EFCore6.Record.ValueObject.Services.Abstractions;
using Dotnet6.EFCore6.Record.ValueObject.Services.Models;
using Dotnet6.EFCore6.Record.ValueObject.Services.Models.Addresses;
using Microsoft.Extensions.Logging;

namespace Dotnet6.EFCore6.Record.ValueObject.Services.Decorators
{
    public class PersonServiceLoggingDecorator : Service<Person, PersonModel, Guid>, IPersonService
    {
        private readonly IPersonService _service;
        private readonly ILogger<PersonServiceLoggingDecorator> _logger;

        public PersonServiceLoggingDecorator(IPersonService service, ILogger<PersonServiceLoggingDecorator> logger, IPersonRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
            : base(repository, unitOfWork, mapper) => (_service, _logger) = (service, logger);

        public async Task DefineAddressAsync(Guid personId, AddressModel model, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Defining new address");
            await _service.DefineAddressAsync(personId, model, cancellationToken);
            _logger.LogInformation("New address defined");
        }
    }
}