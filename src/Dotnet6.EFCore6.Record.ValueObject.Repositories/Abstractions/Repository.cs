using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Dotnet6.EFCore6.Record.ValueObject.Domain.Entities.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Dotnet6.EFCore6.Record.ValueObject.Repositories.Abstractions
{
    public abstract class Repository<TEntity, TId> : IRepository<TEntity, TId>
        where TEntity : Entity<TId>
        where TId : struct
    {
        private readonly DbSet<TEntity> _dbSet;

        protected Repository(DbContext dbDbContext)
        {
            _dbSet = dbDbContext.Set<TEntity>();
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken)
        {
            if (entity is null) return default;
            if (await ExistsAsync(entity.Id, cancellationToken)) return entity;
            var entityEntry = await _dbSet.AddAsync(entity, cancellationToken);
            return entityEntry.Entity;
        }
        
        public virtual Task<bool> ExistsAsync(TId id, CancellationToken cancellationToken)
            => _dbSet.AnyAsync(x => Equals(x.Id, id), cancellationToken);
        
        public async Task<TEntity> GetByIdAsync(TId id, CancellationToken cancellationToken, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = default, bool asTracking = default)
        {
            if (Equals(id, default(TId))) return default;

            return asTracking
                ? include is null
                    ? await _dbSet.FindAsync(new object[] {id}, cancellationToken)
                    : await include(_dbSet).AsTracking().SingleOrDefaultAsync(x => Equals(x.Id, id), cancellationToken)
                : include is null
                    ? await _dbSet.AsNoTracking().SingleOrDefaultAsync(x => Equals(x.Id, id), cancellationToken)
                    : await include(_dbSet).AsNoTracking().SingleOrDefaultAsync(x => Equals(x.Id, id), cancellationToken);
        }

        public virtual async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            if (await ExistsAsync(entity.Id, cancellationToken) is false) return;
            _dbSet.Update(entity);
        }
        
        public Task<List<TEntity>> GetAllAsync(
            CancellationToken cancellationToken,
            Expression<Func<TEntity, bool>> predicate = default,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = default,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = default,
            bool asTracking = default)
        {
            var query = asTracking ? _dbSet.AsTracking() : _dbSet.AsNoTrackingWithIdentityResolution();

            query = include is null ? query : include(query);
            query = predicate is null ? query : query.Where(predicate);
            query = orderBy is null ? query : orderBy(query);

            return query.ToListAsync(cancellationToken);
        }
    }
}