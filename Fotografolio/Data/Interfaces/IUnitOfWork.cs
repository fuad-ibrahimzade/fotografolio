using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http;
using Fotografolio.Data;
using Fotografolio.Data.Models;
using Fotografolio.Services;
using Fotografolio.Data.Repositories;
using Microsoft.Extensions.Options;
using Fotografolio.Helpers;
using System.Collections.Generic;
using Microsoft.AspNetCore.Antiforgery;

namespace Fotografolio.Data.Interfaces
{
    public interface IUnitOfWork
    {
        public IAntiforgery antiforgery { get; }
        static Dictionary<string, int> SeededMaxIds { get; set; }
        void MigrateDatabse(string targetMigration);
        static string HerokuPostgreSqlConnectionString { get; set; }
        string BaseUrl { get; set; }
        FotografolioDbContext _context { get; set; }
        GenericRepository<About> AboutRepository { get; }
        GalleryRepository GalleryRepository { get; }
        GenericRepository<Link> LinkRepository { get; }
        GenericRepository<Logo> LogoRepository { get; }
        PhotoRepository PhotoRepository { get; }
        GenericRepository<Slide> SlideRepository { get; }
        IHttpContextAccessor httpContextAccessor { get; }
        UserManager<ApplicationUser> UserManager { get; }
        RoleManager<IdentityRole> RoleManager { get; }
        SignInManager<ApplicationUser> SignInManager { get; }
        IOptions<AppSettings> appSettings { get; set; }
        UserService UserService { get; }
        string ClientIpAddress();
        string RandomColor();
        bool CreateDefaults();
        Task<int> SaveAsync();
        void Dispose();
    }
}
