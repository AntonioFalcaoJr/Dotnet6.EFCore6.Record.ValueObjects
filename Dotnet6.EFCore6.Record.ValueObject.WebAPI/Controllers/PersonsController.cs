using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Dotnet6.EFCore6.Record.ValueObject.Domain.Entities;
using Dotnet6.EFCore6.Record.ValueObject.Domain.ValueObjects;
using Dotnet6.EFCore6.Record.ValueObject.Repositories;
using Dotnet6.EFCore6.Record.ValueObject.Repositories.UnitsOfWork;
using Dotnet6.EFCore6.Record.ValueObject.WebAPI.Models;
using Dotnet6.EFCore6.Record.ValueObject.WebAPI.Models.Addresses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dotnet6.EFCore6.Record.ValueObject.WebAPI.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PersonsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserRepository _repository;

        public PersonsController(IUserRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
            => (_repository, _unitOfWork, _mapper) = (repository, unitOfWork, mapper);

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Person>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<IEnumerable<Person>>> GetAllAsync(CancellationToken cancellationToken)
        {
            var allPersons = await _repository.GetAllAsync(
                include: persons => persons.Include(person => person.Address),
                cancellationToken: cancellationToken);

            if (allPersons is not {Count: > 0}) return NoContent();
            return Ok(allPersons);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Person))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Person>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            if (id == default) return BadRequest("Invalid identifier.");

            var person = await _repository.GetByIdAsync(
                id: id,
                include: persons => persons.Include(entity => entity.Address),
                cancellationToken: cancellationToken);

            if (person is null) return NotFound();
            return Ok(person);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Person))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Person>> PostAsync([FromBody] PersonModel model, CancellationToken cancellationToken)
        {
            var person = _mapper.Map<Person>(model);
            await _repository.AddAsync(person, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return CreatedAtAction(
                actionName: nameof(GetByIdAsync),
                routeValues: new
                {
                    person.Id,
                    cancellationToken
                },
                value: person);
        }

        [HttpPut("define-address/{personId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Person))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Address>> DefineAddress(Guid personId, [FromBody] AddressModel model, CancellationToken cancellationToken)
        {
            var address = _mapper.Map<AddressModel, Address>(model);

            var person = await _repository.GetByIdAsync(
                id: personId,
                include: persons => persons.Include(entity => entity.Address),
                cancellationToken: cancellationToken,
                asTracking: true);

            if (person is null) return NotFound();

            person.DefineAddress(address);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Ok(address);
        }
    }
}