using Fotografolio.Data.Models;
using Fotografolio.Data.Models.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Fotografolio.Data
{
    public class FotografolioDbContext : IdentityDbContext<ApplicationUser>
    {
        public FotografolioDbContext(DbContextOptions options) : base(options)
        {
        }

        public static Dictionary<string, int> SeededMaxIds { get; set; }
        public static string HerokuPostgreSqlConnectionString { get; set; }
        public static string LocalSQLServerConnectionString { get; set; }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is IEntity && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((IEntity)entityEntry.Entity).updated_at = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((IEntity)entityEntry.Entity).created_at = DateTime.Now;
                }
            }

            var applicationUsers = ChangeTracker
                .Entries()
                .Where(e => e.Entity is ApplicationUser && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));
            foreach (var applicationUser in applicationUsers)
            {
                ((ApplicationUser)applicationUser.Entity).updated_at = DateTime.Now;

                if (applicationUser.State == EntityState.Added)
                {
                    ((ApplicationUser)applicationUser.Entity).created_at = DateTime.Now;
                }
            }

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            if (FotografolioDbContext.HerokuPostgreSqlConnectionString != null)
                builder.HasDefaultSchema("public");//for postgre sql

            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.Entity<Photo>()
                .HasOne(p=>p.Gallery)
                .WithMany(g=>g.Photos)
                .HasForeignKey(photo => photo.gallery_id)
                .OnDelete(DeleteBehavior.Cascade);

            //builder.Entity<Comment>()
            //.Property(e => e.child_comment_ids)
            //.HasConversion(
            //    v => string.Join(',', v),
            //    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList<string>());
            //builder.Entity<CustomAnalytics>()
            //.Property(e => e.analytics_data)
            //.HasConversion(
            //    v => v.ToString(),
            //    v => JObject.Parse(v));
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (FotografolioDbContext.HerokuPostgreSqlConnectionString != null)
        //        optionsBuilder.UseNpgsql(HerokuPostgreSqlConnectionString);
        //    else
        //        optionsBuilder.UseSqlServer(FotografolioDbContext.LocalSQLServerConnectionString);
        //}
        public DbSet<About> About { get; set; }
        public DbSet<Gallery> Gallery { get; set; }
        public DbSet<Link> Link { get; set; }
        public DbSet<Logo> Logo { get; set; }
        public DbSet<Photo> Photo { get; set; }
        public DbSet<Slide> Slide { get; set; }

        public void MigrateDatabse(string targetMigration=null)
        {
            if (targetMigration != null)
                this.GetInfrastructure().GetService<IMigrator>().Migrate(targetMigration);
            else
                this.Database.Migrate();
        }
    }
}
