using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Dotnet6.EFCore6.Record.ValueObject.Domain.Entities.Abstractions;
using Dotnet6.EFCore6.Record.ValueObject.Repositories.Abstractions;
using Dotnet6.EFCore6.Record.ValueObject.Repositories.UnitsOfWork;
using Microsoft.EntityFrameworkCore.Query;

namespace Dotnet6.EFCore6.Record.ValueObject.Services.Abstractions
{
    public abstract class Service<TEntity, TModel, TId> : IService<TEntity, TModel, TId>
        where TEntity : Entity<TId>
        where TModel : class
        where TId : struct
    {
        protected readonly IMapper Mapper;
        protected readonly IRepository<TEntity, TId> Repository;
        protected readonly IUnitOfWork UnitOfWork;

        protected Service(IRepository<TEntity, TId> repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            Repository = repository;
            UnitOfWork = unitOfWork;
            Mapper = mapper;
        }

        public virtual async Task<TEntity> EditAsync(TModel model, CancellationToken cancellationToken)
        {
            var entity = Mapper.Map<TEntity>(model);
            return await OnEditAsync(entity, cancellationToken);
        }

        public virtual async Task<bool> ExistsAsync(TId id, CancellationToken cancellationToken)
            => await Repository.ExistsAsync(id, cancellationToken);

        public virtual async Task<List<TEntity>> GetAllAsync(
            CancellationToken cancellationToken,
            Expression<Func<TEntity, bool>> predicate = default,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = default,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = default,
            bool asTracking = default)
            => await Repository.GetAllAsync(cancellationToken, predicate, orderBy, include, asTracking);

        public virtual async Task<TEntity> GetByIdAsync(TId id, CancellationToken cancellationToken, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = default, bool asTracking = default)
            => await Repository.GetByIdAsync(id, cancellationToken, include, asTracking);

        public virtual async Task<TEntity> SaveAsync(TModel model, CancellationToken cancellationToken)
        {
            var entity = Mapper.Map<TEntity>(model);
            return await OnSaveAsync(entity, cancellationToken);
        }

        protected async Task<TEntity> OnEditAsync(TEntity entity, CancellationToken cancellationToken)
        {
            await Repository.UpdateAsync(entity, cancellationToken);
            return await UnitOfWork.SaveChangesAsync(cancellationToken) ? entity : default;
        }

        protected async Task<TEntity> OnSaveAsync(TEntity entity, CancellationToken cancellationToken)
        {
            await Repository.AddAsync(entity, cancellationToken);
            return await UnitOfWork.SaveChangesAsync(cancellationToken) ? entity : default;
        }
    }
}