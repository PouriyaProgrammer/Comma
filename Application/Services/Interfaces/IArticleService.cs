
namespace Application.Services.Interfaces
{
    public interface IArticleService
    {
        Task<ArticleResult> GetAll(int skip, int take);
        Task<ArticleResult> GetLatestArticles(int skip, int take);
        Task<ArticleResult> Get(int id, bool addViewTime = false);
        Task<ArticleResult> GetBySlug(string slug, bool addViewTime = false);
        Task<CreateArticleStatus> Create(CreateArticle article);
        Task<EditArticleStatus> Edit(EditArticle article);
        Task<bool> IsExists(Expression<Func<Article, bool>> expression);
        Task<ArticleResult> Search(ArticleSearchModel searchModel);
        Task<List<string>> GetAllKeywords();
    }
}
