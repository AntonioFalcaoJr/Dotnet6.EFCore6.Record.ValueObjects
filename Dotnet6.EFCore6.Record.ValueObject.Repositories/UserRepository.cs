using System;
using Dotnet6.EFCore6.Record.ValueObject.Domain.Entities;
using Dotnet6.EFCore6.Record.ValueObject.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Dotnet6.EFCore6.Record.ValueObject.Repositories
{
    public class UserRepository: Repository<Person, Guid>, IUserRepository
    {
        public UserRepository(DbContext dbDbContext) 
            : base(dbDbContext) { }
    }
}