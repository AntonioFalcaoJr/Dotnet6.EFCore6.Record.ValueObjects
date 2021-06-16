using System;

namespace Dotnet6.EFCore6.Record.ValueObject.Domain.Exceptions
{
    public class BusinessException : Exception
    {
        public BusinessException(string message) 
            : base(message) { }
    }
}