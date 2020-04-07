using Fotografolio.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Fotografolio.Data.Repositories
{
    public class GalleryRepository : GenericRepository<Gallery>
    {
        public GalleryRepository(FotografolioDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Gallery>> GetAllAsync()
        {
            return await dbSet.Include(g => g.Photos).ToListAsync();
        }

        public override Task<IEnumerable<Gallery>> GetAsync(Expression<Func<Gallery, bool>> filter = null, Func<IQueryable<Gallery>, IOrderedQueryable<Gallery>> orderBy = null, string includeProperties = "")
        {
            return base.GetAsync(filter, orderBy, typeof(Photo).Name+"s");
        }

        public override async Task<Gallery> GetByIDAsync(object id)
        {
            return await dbSet.Where(g => g.id.ToString() == id.ToString()).Include(g => g.Photos).FirstOrDefaultAsync();
        }
    }
}
