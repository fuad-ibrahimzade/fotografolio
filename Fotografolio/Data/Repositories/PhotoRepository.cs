using Fotografolio.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Fotografolio.Data.Repositories
{
    public class PhotoRepository : GenericRepository<Photo>
    {
        public PhotoRepository(FotografolioDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Photo>> GetAllAsync()
        {
            return await dbSet.Include(p => p.Gallery).ToListAsync();
        }

        public override Task<IEnumerable<Photo>> GetAsync(Expression<Func<Photo, bool>> filter = null, Func<IQueryable<Photo>, IOrderedQueryable<Photo>> orderBy = null, string includeProperties = "")
        {
            return base.GetAsync(filter, orderBy, typeof(Gallery).Name);
        }

        public override async Task<Photo> GetByIDAsync(object id)
        {
            return await dbSet.Where(P => P.id.ToString() == id.ToString()).Include(p => p.Gallery).FirstOrDefaultAsync();
        }
    }
}
