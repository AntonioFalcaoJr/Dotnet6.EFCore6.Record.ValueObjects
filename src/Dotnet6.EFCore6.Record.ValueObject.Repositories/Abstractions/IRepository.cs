using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Dotnet6.EFCore6.Record.ValueObject.Domain.Entities.Abstractions;
using Microsoft.EntityFrameworkCore.Query;

namespace Dotnet6.EFCore6.Record.ValueObject.Repositories.Abstractions
{
    public interface IRepository<TEntity, in TId>
        where TEntity : Entity<TId>
        where TId : struct
    {
        Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken);

        Task<bool> ExistsAsync(TId id, CancellationToken cancellationToken);

        Task<TEntity> GetByIdAsync(TId id, CancellationToken cancellationToken, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = default, bool asTracking = default);

        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);

        Task<List<TEntity>> GetAllAsync(
            CancellationToken cancellationToken,
            Expression<Func<TEntity, bool>> predicate = default,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = default,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = default,
            bool asTracking = default);
    }
}