using Application.DTOs.ArticleCategory;
using Domain.Entities.ArticleCategory;
using System.Linq.Expressions;

namespace Application.Services.Interfaces
{
    public interface IArticleCategoryService
    {
        Task<CreateArticleCategoryStatus> Create(CreateArticleCategory category);
        Task<EditArticleCategoryStatus> Edit(EditArticleCategory category);
        Task<ArticleCategoryResult> GetAll(int skip, int take);
        Task<ArticleCategoryResult> Get(int id);
        Task<ArticleCategoryResult> GetBySlug(string slug);
        Task<ArticleCategoryResult> Search(ArticleCategorySearchDto searchModel);
        Task<List<string>> GetAllKeywords();
        Task<bool> IsExists(Expression<Func<ArticleCategory, bool>> expression);
    }
}
