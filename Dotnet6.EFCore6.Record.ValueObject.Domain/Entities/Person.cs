using System;
using Dotnet6.EFCore6.Record.ValueObject.Domain.Entities.Abstractions;
using Dotnet6.EFCore6.Record.ValueObject.Domain.Exceptions;
using Dotnet6.EFCore6.Record.ValueObject.Domain.ValueObjects;

namespace Dotnet6.EFCore6.Record.ValueObject.Domain.Entities
{
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
            if(address.Equals(Address)) return;
            Address = address;
        }
    }
}