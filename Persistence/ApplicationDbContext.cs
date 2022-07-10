using Application.ContextContracts;
using Domain.Entities.Article;
using Domain.Entities.ArticleCategory;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>, IApplicationContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<ArticleCategory> ArticleCategories { get; set; }
        public DbSet<Article> Articles { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ArticleCategory>()
                .HasMany(x => x.Articles)
                .WithOne(x => x.ArticleCategory)
                .HasForeignKey(x => x.CategoryId);

            builder.Entity<Article>()
                .HasKey(x => x.Id);

            builder.Entity<ArticleCategory>()
                .HasKey(x => x.Id);

            base.OnModelCreating(builder);
        }
    }
}