using System;
using Dotnet6.EFCore6.Record.ValueObject.Domain.Entities;
using Dotnet6.EFCore6.Record.ValueObject.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Dotnet6.EFCore6.Record.ValueObject.Repositories
{
    public class PersonRepository: Repository<Person, Guid>, IPersonRepository
    {
        public PersonRepository(DbContext dbDbContext) 
            : base(dbDbContext) { }
    }
}