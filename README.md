# Dotnet6.EFCore6.Record.ValueObjects

This project aims to demonstrate the configuration and use of **Records** as **Value Objects** with **EF Core 6**.

### Give a Star! :star:

---

## Value Objects

A value object is a small object that represents a simple entity whose equality is not based on identity. Two value objects are equal when they have the same value, not necessarily being the same object

 Main characteristics:

- They have no identity.
- They are immutable.

## Records

Defines a **reference type** that provides built-in functionality for encapsulating data. Is possible to create `record` types with **immutable properties**.

### Immutability 

A `record` type is not necessarily immutable. Is possible to declare properties with set accessors and fields that aren't readonly. But while records can be mutable, they make it easier to create immutable data models.

### Equality

Value equality means that two variables of a `record` type are equal if the types match and all property and field values match. For other reference types, equality means identity. That is, two variables of a reference type are equal if they refer to the same object.

For **value objects** as `record` types, the synthesized equality members by default would be something like:

```c#
class R1 : IEquatable<R1>
{
    public T1 P1 { get; init; }
    
    protected virtual Type EqualityContract => typeof(R1);
    
    public override bool Equals(object? obj) => Equals(obj as R1);
    
    public virtual bool Equals(R1? other)
    {
        return !(other is null) &&
            EqualityContract == other.EqualityContract &&
            EqualityComparer<T1>.Default.Equals(P1, other.P1);
    }
    
    public static bool operator==(R1? left, R1? right)
        => (object)left == right || (left?.Equals(right) ?? false);
        
    public static bool operator!=(R1? left, R1? right)
        => !(left == right);
        
    public override int GetHashCode()
    {
        return Combine(EqualityComparer<Type>.Default.GetHashCode(EqualityContract),
            EqualityComparer<T1>.Default.GetHashCode(P1));
    }
}
```
## Persist value objects as owned entity types in EF Core

> Even with some gaps between the canonical value object pattern in DDD and the owned entity type in EF Core, **it's currently the best way to persist value objects with EF Core**.

An owned entity type allows you to map types that do not have their own identity explicitly defined in the domain model and are used as properties, such as a **value object**. 

The identity of instances of owned types is not completely their own. It consists of three components:

- The identity of the owner
- The navigation property pointing to them
- In the case of collections of owned types, an independent component.

### Entity

```c#
public class Person : Entity<Guid>
{
    public Person(string name, int age) 
        => (Name, Age) = (name, age);

    public string Name { get; }
    public int Age { get; }
    public Address Address { get; private set; }

    public void DefineAddress(Address address)
    {
        if (address is null) throw new BusinessException("Home address must be informed");
        // Benefit of the record Equality Contract
        if(address.Equals(Address)) return;
        Address = address;
    }
}
```

### Value object

```c#
public record Address : Abstractions.ValueObject
{
    // Empty constructor in this case is required by EF Core,
    // because has a complex type as a parameter in the default constructor.
    private Address() { }

    public Address(Street street, string zipCode)
        => (Street, ZipCode) = (street, zipCode);

    // The setter (init) is required by mapping engine for complex types.
    public Street Street { get; private init; }
    public string ZipCode { get; }
}
```
### Configuring 

To solve the relationship required for the relation database, the identifier has to be defined as a **shadow property**.

```c#
public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.HasKey(user => user.Id);

        ...

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
                    .HasKey("Id"); // Shadow Primary Key

                // Configures a relationship where the Street is owned by (or part of) Addresses.
                // In this case, is not used "ToTable();" to maintain the owned and owner in the same table. 
                addressNavigationBuilder.OwnsOne(
                    address => address.Street,
                    streetNavigationBuilder =>
                    {
                        ...

                        // Configures a relationship where the City is owned by (or part of) Street.
                        // In this case, is not used "ToTable();" to maintain the owned and owner in the same table. 
                        streetNavigationBuilder.OwnsOne(
                            street => street.City,
                            cityNavigationBuilder =>
                            {
                                ...
                            });
                    });
            });
    }
}
```
### Script produced

```sql
CREATE TABLE [Persons]
(
    [Id]   uniqueidentifier NOT NULL,
    [Name] varchar(128)     NOT NULL,
    [Age]  int              NOT NULL,
    CONSTRAINT [PK_Persons] PRIMARY KEY ([Id])
);

CREATE TABLE [Addresses]
(
    [Id]                                 uniqueidentifier NOT NULL,
    [Street_City_Name]                   varchar(128)     NULL,
    [Street_City_State_Country_Initials] varchar(8)       NULL,
    [Street_City_State_Country_Name]     varchar(128)     NULL,
    [Street_City_State_Initials]         varchar(8)       NULL,
    [Street_City_State_Name]             varchar(128)     NULL,
    [Street_Name]                        varchar(128)     NULL,
    [Street_Number]                      int              NULL,
    [ZipCode]                            varchar(32)      NULL,
    [PersonId]                           uniqueidentifier NOT NULL,
    CONSTRAINT [PK_Addresses] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Addresses_Persons_PersonId] FOREIGN KEY ([PersonId]) REFERENCES [Persons] ([Id]) ON DELETE CASCADE
);

CREATE UNIQUE INDEX [IX_Addresses_PersonId] ON [Addresses] ([PersonId]);
```

## Deleting orphans Value Objects with EF Core

The relationship between parent and child entities may be required or optional. A required relationship means that the child cannot exist without a parent, and if the parent is deleted or the relationship between the child and the parent is severed, then the child becomes orphaned. In this case, EF Core will perform a automatically child deletion.

## Running

The [`./docker-compose.yml`](./docker-compose.yml) provide the `WebAPI` and `MS SQL Server` applications:

```bash
docker-compose up -d
``` 

## Built With

### Microsoft Stack - v6.0 - preview 4

* [.NET 6](https://devblogs.microsoft.com/dotnet/announcing-net-6-preview-4/) - Base framework;
* [ASP.NET 6](https://devblogs.microsoft.com/aspnet/asp-net-core-updates-in-net-6-preview-4/) - Web framework;
* [Entity Framework Core 6](https://devblogs.microsoft.com/dotnet/announcing-entity-framework-core-6-0-preview-4-performance-edition/) - ORM;
* [Microsoft SQL Server on Linux for Docker](https://hub.docker.com/_/microsoft-mssql-server) - Database.

### Community Stack

* [AutoMapper](https://automapper.org/) - A convention-based object-object mapper;
* [Bogus](https://github.com/bchavez/Bogus) - A simple and sane fake data generator for C#, F#, and VB.NET;
* [Serilog](https://serilog.net/) - Serilog provides diagnostic logging to files, the console, and elsewhere.
