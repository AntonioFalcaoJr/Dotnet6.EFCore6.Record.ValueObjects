using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dotnet6.EFCore6.Record.ValueObject.Domain.Entities;
using Dotnet6.EFCore6.Record.ValueObject.Services;
using Dotnet6.EFCore6.Record.ValueObject.Services.Models;
using Dotnet6.EFCore6.Record.ValueObject.Services.Models.Addresses;
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
        private readonly IPersonService _service;

        public PersonsController(IPersonService service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Person>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<IEnumerable<Person>>> GetAllAsync(CancellationToken cancellationToken)
        {
            var allPersons = await _service.GetAllAsync(
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

            var person = await _service.GetByIdAsync(
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
            var person = await _service.SaveAsync(model, cancellationToken);

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
        public async Task<ActionResult<AddressModel>> DefineAddressAsync(Guid personId, [FromBody] AddressModel model, CancellationToken cancellationToken)
        {
            await _service.DefineAddressAsync(personId, model, cancellationToken);
            return Ok(model);
        }
    }
}