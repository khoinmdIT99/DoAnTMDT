using Infrastructure.Database;
using Infrastructure.Database.Entities;
using Infrastructure.Database.Models;
using Infrastructure.Database.Paging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Database
{
    public class Repository<TContext, TEntity> : BaseRepository<TContext, TEntity>, IRepository<TEntity>
        where TEntity : class
        where TContext : DbContext, new()
    {
        readonly IUnitOfWork<TContext> _unitOfWork;
        public Repository(IUnitOfWork<TContext> unitOfWork) : base(unitOfWork.Context)
        {
            this._unitOfWork = unitOfWork;
        }

        public TEntity Get(params object[] keyValues)
        {
            return DbSet.Find(keyValues);
        }

        public void Add(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public void Add(params TEntity[] entities)
        {
            DbSet.AddRange(entities);
        }


        public void Add(IEnumerable<TEntity> entities)
        {
            DbSet.AddRange(entities);
        }

        public void BulkInsert(IList<TEntity> entities)
        {
            DbSet.AddRange(entities);
            //throw new NotImplementedException();
        }

        public void Remove(TEntity entity)
        {
            if (entity != null) DbSet.Remove(entity);
        }

        public void Delete(TEntity entity)
        {
            if (entity != null) DbSet.Remove(entity);
        }


        public void Delete(object id)
        {
            var typeInfo = typeof(TEntity).GetTypeInfo();
            var key = DbContext.Model.FindEntityType(typeInfo).FindPrimaryKey().Properties.FirstOrDefault();
            var property = typeInfo.GetProperty(key?.Name ?? throw new InvalidOperationException());
            if (property != null)
            {
                var entity = Activator.CreateInstance<TEntity>();
                property.SetValue(entity, id);
                DbContext.Entry(entity).State = EntityState.Deleted;
            }
            else
            {
                var entity = DbSet.Find(id);
                if (entity != null) Delete(entity);
            }
        }

        public void Delete(params TEntity[] entities)
        {
            DbSet.RemoveRange(entities);
        }

        public void Delete(IEnumerable<TEntity> entities)
        {
            DbSet.RemoveRange(entities);
        }


        [Obsolete("Method is replaced by GetList")]
        public IEnumerable<TEntity> Get()
        {
            return DbSet.AsEnumerable();
        }

        [Obsolete("Method is replaced by GetList")]
        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Where(predicate).AsEnumerable();
        }

        public void Update(TEntity entity)
        {
            DbSet.Update(entity);
        }

        public void Update(params TEntity[] entities)
        {
            DbSet.UpdateRange(entities);
        }


        public void Update(IEnumerable<TEntity> entities)
        {
            DbSet.UpdateRange(entities);
        }

        public void Dispose()
        {
            DbContext?.Dispose();
        }

        public async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, bool disableTracking = true)
        {
            IQueryable<TEntity> query = DbSet;
            if (disableTracking) query = query.AsNoTracking();

            if (include != null) query = include(query);

            if (predicate != null) query = query.Where(predicate);

            if (orderBy != null)
                return await orderBy(query).FirstOrDefaultAsync();
            return await query.FirstOrDefaultAsync();
        }

        public Task<IPaginate<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            int index = 0,
            int size = 20,
            bool disableTracking = true,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            IQueryable<TEntity> query = DbSet;
            if (disableTracking) query = query.AsNoTracking();

            if (include != null) query = include(query);

            if (predicate != null) query = query.Where(predicate);

            if (orderBy != null)
                return orderBy(query).ToPaginateAsync(index, size, 0, cancellationToken);
            return query.ToPaginateAsync(index, size, 0, cancellationToken);
        }

        public ValueTask<EntityEntry<TEntity>> AddAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            return DbSet.AddAsync(entity, cancellationToken);
        }

        public Task AddAsync(params TEntity[] entities)
        {
            return DbSet.AddRangeAsync(entities);
        }


        public Task AddAsync(IEnumerable<TEntity> entities,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return DbSet.AddRangeAsync(entities, cancellationToken);
        }


        [Obsolete("Use get list ")]
        public IEnumerable<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public void UpdateAsync(TEntity entity)
        {
            DbSet.Update(entity);
        }

        public ValueTask<EntityEntry<TEntity>> AddAsync(TEntity entity)
        {
            return AddAsync(entity, new CancellationToken());
        }

        public void BulkDelete(IList<TEntity> entities)
        {
            DbSet.RemoveRange(entities);
            //throw new NotImplementedException();
        }

        public int Save(RequestContext requestContext = null)
        {
            return _unitOfWork.Save(requestContext);
        }

        public Task SaveAsync(RequestContext requestContext = null)
        {
            return _unitOfWork.SaveAsync(requestContext);
        }

		public IDbContextTransaction BeginTransaction()
		{
            return _unitOfWork.BeginTransaction();
		}
	}
}
