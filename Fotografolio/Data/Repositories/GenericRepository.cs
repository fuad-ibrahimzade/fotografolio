using Fotografolio.Data.Models.Interfaces;
using Fotografolio.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace Fotografolio.Data.Repositories
{
    public class GenericRepository<TEntity> : Interfaces.IBaseRepository<TEntity> where TEntity : class, IEntity
    {
        internal FotografolioDbContext context;
        internal DbSet<TEntity> dbSet;

        public GenericRepository(FotografolioDbContext context)
        {
            //, [System.Runtime.CompilerServices.CallerMemberName] string memberName = ""
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public virtual async Task<TEntity> GetByIDAsync(object id)
        {
            return await dbSet.FindAsync(id);
        }

        public virtual async Task<bool> IsEmpty()
        {
            return !await dbSet.AnyAsync();
        }

        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
            //await SaveAsync();
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
            //await SaveAsync();
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.FindAsync(id).GetAwaiter().GetResult();
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
            //await SaveAsync();
        }

        public IEnumerable<object> GetRandomLimit(int limit)
        {
            var entities = dbSet.AsEnumerable<TEntity>();
            Random rnd = new Random();
            return entities.OrderBy(r => rnd.Next()).Take(limit).ToList();
        }

        public async Task<object> GetDistinctColumnAsync(string column)
        {
            var columnData = await dbSet.Select($"new {typeof(TEntity).GetType().ToString()} {{ {column} = p.{column} }}").ToDynamicListAsync();
            return columnData;
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
