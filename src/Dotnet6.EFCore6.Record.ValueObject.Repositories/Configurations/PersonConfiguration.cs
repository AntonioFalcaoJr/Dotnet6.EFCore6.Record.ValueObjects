using System;
using Dotnet6.EFCore6.Record.ValueObject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dotnet6.EFCore6.Record.ValueObject.Repositories.Configurations
{
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder
                .HasKey(user => user.Id);

            builder
                .Property(user => user.Age)
                .IsRequired();

            builder
                .Property(user => user.Name)
                .IsUnicode(false)
                .HasMaxLength(128)
                .IsRequired();

            // A value object represents a simple entity whose equality is not based on identity,
            // based on that, the identifier is defined as a shadow.

            // Configures a relationship where the Address is owned by (or part of) Person.
            builder.OwnsOne(
                person => person.Address,
                addressNavigationBuilder =>
                {
                    // Configures a different table that the entity type maps to when targeting a relational database.
                    addressNavigationBuilder
                        .ToTable("Addresses");

                    // Configures the relationship to the owner, and indicates the Foreign Key.
                    addressNavigationBuilder
                        .WithOwner()
                        .HasForeignKey("PersonId"); // Shadow Foreign Key

                    // Configure a property of the owned entity type, in this case the to be used as Primary Key
                    addressNavigationBuilder
                        .Property<Guid>("Id"); // Shadow property

                    // Sets the properties that make up the primary key for this owned entity type.
                    addressNavigationBuilder
                        .HasKey("Id"); // Shadow Key
                    
                    addressNavigationBuilder
                        .Property(address => address.ZipCode)
                        .HasMaxLength(32)
                        .IsRequired()
                        .IsUnicode(false);

                    // Configures a relationship where the Street is owned by (or part of) Addresses.
                    addressNavigationBuilder.OwnsOne(
                        address => address.Street,
                        streetNavigationBuilder =>
                        {
                            streetNavigationBuilder
                                .Property(street => street.Name)
                                .HasMaxLength(128)
                                .IsRequired()
                                .IsUnicode(false);

                            streetNavigationBuilder
                                .Property(street => street.Number)
                                .IsRequired();

                            // Configures a relationship where the City is owned by (or part of) Street.
                            streetNavigationBuilder.OwnsOne(
                                street => street.City,
                                cityNavigationBuilder =>
                                {
                                    cityNavigationBuilder
                                        .Property(city => city.Name)
                                        .IsUnicode(false)
                                        .HasMaxLength(128)
                                        .IsRequired();

                                    // Configures a relationship where the State is owned by (or part of) City.
                                    cityNavigationBuilder.OwnsOne(
                                        city => city.State,
                                        stateNavigationBuilder =>
                                        {
                                            stateNavigationBuilder
                                                .Property(country => country.Initials)
                                                .HasMaxLength(8)
                                                .IsRequired()
                                                .IsUnicode(false);

                                            stateNavigationBuilder
                                                .Property(country => country.Name)
                                                .HasMaxLength(128)
                                                .IsRequired()
                                                .IsUnicode(false);

                                            // Configures a relationship where the Country is owned by (or part of) State.
                                            stateNavigationBuilder.OwnsOne(
                                                state => state.Country,
                                                countryNavigationBuilder =>
                                                {
                                                    countryNavigationBuilder
                                                        .Property(country => country.Initials)
                                                        .HasMaxLength(8)
                                                        .IsRequired()
                                                        .IsUnicode(false);

                                                    countryNavigationBuilder
                                                        .Property(country => country.Name)
                                                        .HasMaxLength(128)
                                                        .IsRequired()
                                                        .IsUnicode(false);
                                                });
                                        });
                                });
                        });
                });
        }
    }
}