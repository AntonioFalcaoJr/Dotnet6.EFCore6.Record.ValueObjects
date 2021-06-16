using System;
using System.Linq;
using Bogus;
using Microsoft.EntityFrameworkCore;

namespace Dotnet6.EFCore6.Record.ValueObject.Repositories.Contexts.Extensions
{
    public static class ModelBuilderExtensions
    {
        private const int Amount = 10;
        private static readonly Faker Faker = new();

        public static void Seed(this ModelBuilder modelBuilder)
            => modelBuilder.SeedPersons();

        private static void SeedPersons(this ModelBuilder modelBuilder) =>
            modelBuilder
                .Entity<Domain.Entities.Person>()
                .HasData(Enumerable
                    .Range(0, Amount)
                    .Select(_ => new
                    {
                        Id = Guid.NewGuid(),
                        Name = Faker.Name.FirstName(),
                        Age = Faker.Random.Int(0, 80),
                    }));
    }
}