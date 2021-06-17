using System;
using Dotnet6.EFCore6.Record.ValueObject.Domain.Entities;
using Dotnet6.EFCore6.Record.ValueObject.Repositories.Abstractions;

namespace Dotnet6.EFCore6.Record.ValueObject.Repositories
{
    public interface IPersonRepository : IRepository<Person, Guid> { }
}