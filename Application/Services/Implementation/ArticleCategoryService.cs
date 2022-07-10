using Application.ContextContracts;
using Application.DTOs.ArticleCategory;
using Application.Services.Interfaces;
using Domain.Entities.ArticleCategory;
using Org.BouncyCastle.Math.EC.Rfc7748;
using System.Data.Common;
using System.Linq.Expressions;

namespace Application.Services.Implementation
{
    public class ArticleCategoryService : IArticleCategoryService
    {
        private readonly IApplicationContext _context;

        public ArticleCategoryService(IApplicationContext context)
        {
            _context = context;
        }

        public async Task<CreateArticleCategoryStatus> Create(CreateArticleCategory category)
        => await Task.Run(async () =>
        {
            try
            {
                bool isExists = await IsExists(x => x.Name == category.Name);

                if (isExists)
                {
                    return CreateArticleCategoryStatus.CategoryIsExists;
                }

                ArticleCategory articleCategory = new ArticleCategory()
                {
                    Name = category.Name,
                    Keywords = category.Keywords,
                    Slug = category.Slug,
                    PictureAlt = category.PictureAlt,
                    PictureTitle = category.PictureTitle,
                    ShortDescription = category.ShortDescription,
                };

                try
                {
                    articleCategory.Picture = category.Picture.Upload(category.Slug);
                }
                catch (Exception ex)
                {
                    await File.AppendAllTextAsync(
                        Path.Combine("wwwroot", "Logs.txt"), ex.ToString());
                    return CreateArticleCategoryStatus.PictureNotSave;
                }

                await _context.ArticleCategories.AddAsync(articleCategory);
                await _context.SaveChangesAsync();
                return CreateArticleCategoryStatus.Success;
            }
            catch (Exception ex)
            {
                await File.AppendAllTextAsync(
                        Path.Combine("wwwroot", "Logs.txt"), ex.ToString());
                return CreateArticleCategoryStatus.Error;
            }
        });

        public Task<EditArticleCategoryStatus> Edit(EditArticleCategory category)
        => Task.Run(async () =>
        {
            try
            {
                bool isExists = await IsExists(x => x.Id == category.Id);

                if (!isExists)
                {
                    return EditArticleCategoryStatus.CategoryNotFound;
                }

                isExists = await IsExists(x => x.Name == category.Name
                && x.Id != category.Id);

                if (isExists)
                {
                    return EditArticleCategoryStatus.DuplicatedCategory;
                }

                ArticleCategory articleCategory = new ArticleCategory()
                {
                    Id = category.Id,
                    Name = category.Name,
                    Keywords = category.Keywords,
                    Slug = category.Slug,
                    PictureAlt = category.PictureAlt,
                    PictureTitle = category.PictureTitle,
                    ShortDescription = category.ShortDescription,
                };

                try
                {
                    var befoureCategory = await _context.ArticleCategories.FindAsync(category.Id);
                    string picture = category.Picture.Upload(category.Slug,
                        befoureCategory.Picture);

                    if (picture != "")
                    {
                        articleCategory.Picture = picture;
                    }
                }
                catch (Exception ex)
                {
                    await File.AppendAllTextAsync(
                                            Path.Combine("wwwroot", "Logs.txt"), ex.ToString());
                    return EditArticleCategoryStatus.PictureNotSave;
                }

                _context.ArticleCategories.Update(articleCategory);
                await _context.SaveChangesAsync();

                return EditArticleCategoryStatus.Success;
            }
            catch (Exception ex)
            {
                await File.AppendAllTextAsync(
                        Path.Combine("wwwroot", "Logs.txt"), ex.ToString());
                return EditArticleCategoryStatus.Error;
            }
        });

        public async Task<ArticleCategoryResult> Get(int id)
        => await Task.Run(async () =>
        {
            try
            {
                var category = await _context.ArticleCategories.FindAsync(id);

                return new ArticleCategoryResult
                {
                    Status = ArticleCategoryStatus.Success,
                    Result = new ArticleCategoryDto
                    {
                        Id = id,
                        CountOfArticles = category.CountOfArticles,
                        Keywords = category.Keywords,
                        Name = category.Name,
                        Picture = category.Picture,
                        PictureAlt = category.PictureAlt,
                        PictureTitle = category.PictureTitle,
                        ShortDescription = category.ShortDescription,
                        Slug = category.Slug,
                        CreationDate = category.CreationDate.ToPersianDate()
                    }
                };
            }
            catch (Exception ex)
            {
                await File.AppendAllTextAsync(
                        Path.Combine("wwwroot", "Logs.txt"), ex.ToString());

                return new ArticleCategoryResult
                {
                    Result = new { },
                    Status = ArticleCategoryStatus.Error
                };
            }
        });

        public async Task<ArticleCategoryResult> GetAll(int skip, int take)
        => await Task.Run(async () =>
        {
            try
            {
                var categories = _context.ArticleCategories
                .Skip(skip)
                .Take(take)
                .ToList();

                return new ArticleCategoryResult
                {
                    Result = categories.Select(x => new ArticleCategoryDto
                    {
                        Id = x.Id,
                        CountOfArticles = x.CountOfArticles,
                        Keywords = x.Keywords,
                        Name = x.Name,
                        Picture = x.Picture,
                        PictureAlt = x.PictureAlt,
                        PictureTitle = x.PictureTitle,
                        ShortDescription = x.ShortDescription,
                        Slug = x.Slug,
                        CreationDate = x.CreationDate.ToPersianDate()
                    }),
                    Status = ArticleCategoryStatus.Success
                };
            }
            catch (Exception ex)
            {
                await File.AppendAllTextAsync(
                                        Path.Combine("wwwroot", "Logs.txt"), ex.ToString());

                return new ArticleCategoryResult
                {
                    Result = new { },
                    Status = ArticleCategoryStatus.Error
                };
            }
        });

        public async Task<List<string>> GetAllKeywords()
        => await Task.Run(async () =>
        {
            try
            {
                var categories = await _context.ArticleCategories.ToListAsync();
                return categories.Select(x => x.Keywords).ToList();
            }
            catch (Exception ex)
            {
                await File.AppendAllTextAsync(
                                        Path.Combine("wwwroot", "Logs.txt"), ex.ToString());

                return new List<string>();
            }
        });

        public async Task<ArticleCategoryResult> GetBySlug(string slug)
        => await Task.Run(async () =>
        {
            try
            {
                var category = await _context.ArticleCategories
                    .FirstOrDefaultAsync(x => x.Slug == slug);

                return new ArticleCategoryResult
                {
                    Status = ArticleCategoryStatus.Success,
                    Result = new ArticleCategoryDto
                    {
                        Id = category.Id,
                        CountOfArticles = category.CountOfArticles,
                        Keywords = category.Keywords,
                        Name = category.Name,
                        Picture = category.Picture,
                        PictureAlt = category.PictureAlt,
                        PictureTitle = category.PictureTitle,
                        ShortDescription = category.ShortDescription,
                        Slug = category.Slug,
                        CreationDate = category.CreationDate.ToPersianDate()
                    }
                };
            }
            catch (Exception ex)
            {
                await File.AppendAllTextAsync(
                                                        Path.Combine("wwwroot", "Logs.txt"), ex.ToString());

                return new ArticleCategoryResult
                {
                    Result = new { },
                    Status = ArticleCategoryStatus.Error
                };
            }
        });

        public async Task<bool> IsExists(Expression<Func<ArticleCategory, bool>> expression)
        => await Task.Run(async () =>
        {
            return await _context.ArticleCategories.AnyAsync(expression);
        });

        public Task<ArticleCategoryResult> Search(ArticleCategorySearchDto searchModel)
        => Task.Run(async () =>
        {
            try
            {
                var categories = await _context.ArticleCategories.ToListAsync();

                ArticleCategoryResult articleCategoryResult = new ArticleCategoryResult
                {
                    Result = categories.Select(x => new ArticleCategoryDto
                    {
                        Id = x.Id,
                        CountOfArticles = x.CountOfArticles,
                        Keywords = x.Keywords,
                        Name = x.Name,
                        Picture = x.Picture,
                        PictureAlt = x.PictureAlt,
                        PictureTitle = x.PictureTitle,
                        ShortDescription = x.ShortDescription,
                        Slug = x.Slug,
                        CreationDate = x.CreationDate.ToPersianDate()
                    }),
                    Status = ArticleCategoryStatus.Success
                };

                if (!string.IsNullOrWhiteSpace(searchModel.Name))
                    articleCategoryResult = new ArticleCategoryResult
                    {
                        Result = categories.Select(x => new ArticleCategoryDto
                        {
                            Id = x.Id,
                            CountOfArticles = x.CountOfArticles,
                            Keywords = x.Keywords,
                            Name = x.Name,
                            Picture = x.Picture,
                            PictureAlt = x.PictureAlt,
                            PictureTitle = x.PictureTitle,
                            ShortDescription = x.ShortDescription,
                            Slug = x.Slug,
                            CreationDate = x.CreationDate.ToPersianDate()
                        }).Where(x => x.Name.ToLower().Contains(searchModel.Name.ToLower()))
                        .OrderByDescending(x => x.Id).ToList(),
                        Status = ArticleCategoryStatus.Success
                    };

                if (!string.IsNullOrWhiteSpace(searchModel.Keywords))
                    articleCategoryResult = new ArticleCategoryResult
                    {
                        Result = categories.Select(x => new ArticleCategoryDto
                        {
                            Id = x.Id,
                            CountOfArticles = x.CountOfArticles,
                            Keywords = x.Keywords,
                            Name = x.Name,
                            Picture = x.Picture,
                            PictureAlt = x.PictureAlt,
                            PictureTitle = x.PictureTitle,
                            ShortDescription = x.ShortDescription,
                            Slug = x.Slug,
                            CreationDate = x.CreationDate.ToPersianDate()
                        }).Where(x => x.Keywords.ToLower() == searchModel.Keywords.ToLower())
                        .OrderByDescending(x => x.Id).ToList(),
                        Status = ArticleCategoryStatus.Success
                    };

                return articleCategoryResult;
            }
            catch (Exception ex)
            {
                await File.AppendAllTextAsync(
                                                        Path.Combine("wwwroot", "Logs.txt"), ex.ToString());

                return new ArticleCategoryResult
                {
                    Result = new { },
                    Status = ArticleCategoryStatus.Error
                };
            }
        });
    }
}
