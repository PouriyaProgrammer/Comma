using Domain.Entities.ArticleCategory;

namespace Application.ContextContracts
{
    public interface IApplicationContext
    {
        public DbSet<ArticleCategory> ArticleCategories { get; set; }
        public DbSet<Article> Articles { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
