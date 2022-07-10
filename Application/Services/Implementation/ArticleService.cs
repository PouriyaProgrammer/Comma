using Application.ContextContracts;
using Application.DTOs.ArticleCategory;
using Application.Services.Interfaces;
using Domain.Entities.ArticleCategory;
using Org.BouncyCastle.Math.EC.Rfc7748;

namespace Application.Services.Implementation
{
    public class ArticleService : IArticleService
    {
        private readonly IApplicationContext _context;

        public ArticleService(IApplicationContext context)
        {
            _context = context;
        }

        public async Task<CreateArticleStatus> Create(CreateArticle article)
        => await Task.Run(async () =>
        {
            try
            {
                var isExists = await IsExists(x => x.Title == article.Title);

                if (isExists)
                {
                    return CreateArticleStatus.ArticleIsExsits;
                }

                var category = await _context.ArticleCategories.FindAsync(article.CategoryId);

                if (category == null)
                {
                    return CreateArticleStatus.CategoryNotFound;
                }

                Article article2 = new Article()
                {
                    Title = article.Title,
                    CategoryId = article.CategoryId,
                    Content = article.Content,
                    Keywords = article.Keywords,
                    PictureAlt = article.PictureAlt,
                    PictureTitle = article.PictureTitle,
                    ShortDescription = article.ShortDescription,
                    Slug = article.Slug,
                    TimeToRead = article.TimeToRead,
                    ViewedTime = 0,
                    ArticleCategory = category
                };

                if (article.Picture != null)
                {
                    try
                    {
                        article2.Picture = article.Picture.Upload(article.Slug);
                    }
                    catch (Exception ex)
                    {
                        await File.AppendAllTextAsync(
                            Path.Combine("wwwroot", "Logs.txt"), ex.ToString());
                        return CreateArticleStatus.PictureNotSave;
                    }
                }
                else
                {
                    return CreateArticleStatus.PictureIsNull;
                }

                await _context.Articles.AddAsync(article2);
                await _context.SaveChangesAsync();

                return CreateArticleStatus.Success;
            }
            catch (Exception ex)
            {
                await File.AppendAllTextAsync(
                        Path.Combine("wwwroot", "Logs.txt"), ex.ToString());
                return CreateArticleStatus.Error;
            }
        });

        public async Task<EditArticleStatus> Edit(EditArticle article)
        => await Task.Run(async () =>
        {
            try
            {
                var isExistsArticle = await IsExists(x => x.Id == article.Id);

                if (!isExistsArticle)
                {
                    return EditArticleStatus.ArticleIsNotExists;
                }

                var category = await _context.ArticleCategories
                .FirstOrDefaultAsync(x=>x.Id == article.CategoryId);

                if (category == null)
                {
                    return EditArticleStatus.CategoryNotFound;
                }

                var isDuplicatedRecord = await IsExists(x => x.Title == article.Title &&
                x.Id != article.Id);

                if (isDuplicatedRecord)
                {
                    return EditArticleStatus.DuplicatedArticle;
                }

                //Article article2 = new Article()
                //{
                //    Id = article.Id,
                //    Title = article.Title,
                //    CategoryId = article.CategoryId,
                //    Content = article.Content,
                //    Keywords = article.Keywords,
                //    PictureAlt = article.PictureAlt,
                //    PictureTitle = article.PictureTitle,
                //    ShortDescription = article.ShortDescription,
                //    Slug = article.Slug,
                //    TimeToRead = article.TimeToRead,
                //    ViewedTime = 0,
                //    ArticleCategory = category
                //};

                var befoureArticle = await _context.Articles.FindAsync(article.Id);
                befoureArticle.ArticleCategory = category;
                befoureArticle.CategoryId = article.CategoryId;
                befoureArticle.Content = article.Content;
                befoureArticle.Keywords = article.Keywords;
                befoureArticle.Id = article.Id;
                befoureArticle.PictureAlt = article.PictureAlt;
                befoureArticle.PictureTitle = article.PictureTitle;
                befoureArticle.ShortDescription = article.ShortDescription;
                befoureArticle.Slug = article.Slug;
                befoureArticle.TimeToRead = article.TimeToRead;
                befoureArticle.Title = article.Title;

                try
                {
                    string picture = article.Picture.Upload(category.Slug,
                        befoureArticle.Picture);

                    if (picture != "")
                    {
                        befoureArticle.Picture = picture;
                    }
                }
                catch (Exception ex)
                {
                    await File.AppendAllTextAsync(
                                            Path.Combine("wwwroot", "Logs.txt"), ex.ToString());
                    return EditArticleStatus.PictureNotSave;
                }

                _context.Articles.Update(befoureArticle);
                await _context.SaveChangesAsync();

                return EditArticleStatus.Success;
            }
            catch (Exception ex)
            {
                await File.AppendAllTextAsync(
                                        Path.Combine("wwwroot", "Logs.txt"), ex.ToString());
                return EditArticleStatus.Error;
            }
        });

        public async Task<ArticleResult> Get(int id, bool addViewTime = false)
        => await Task.Run(async () =>
        {
            try
            {
                var article = await _context.Articles.FindAsync(id);

                return new ArticleResult
                {
                    Result = new ArticleDto
                    {
                        Title = article.Title,
                        CategoryId = article.CategoryId,
                        Content = article.Content,
                        CreationDate = article.CreationDate.ToPersianDate(),
                        Id = article.Id,
                        Keywords = article.Keywords,
                        Picture = article.Picture,
                        PictureAlt = article.PictureAlt,
                        PictureTitle = article.PictureTitle,
                        ShortDescription = article.ShortDescription,
                        Slug = article.Slug,
                        TimeToRead = article.TimeToRead,
                        ViewedTime = article.ViewedTime + 1
                    },
                    Status = ArticleStatus.Success
                };
            }
            catch (Exception ex)
            {
                await File.AppendAllTextAsync(
                                        Path.Combine("wwwroot", "Logs.txt"), ex.ToString());

                return new ArticleResult
                {
                    Result = new { },
                    Status = ArticleStatus.Error
                };
            }
        });

        public async Task<ArticleResult> GetAll(int skip, int take)
        => await Task.Run(async () =>
        {
            try
            {
                return new ArticleResult
                {
                    Result = await _context.Articles
                .Skip(skip)
                .Take(take)
                .Select(x => new ArticleDto
                {
                    Id = x.Id,
                    CategoryId = x.CategoryId,
                    Content = x.Content,
                    CreationDate = x.CreationDate.ToPersianDate(),
                    Keywords = x.Keywords,
                    Picture = x.Picture,
                    PictureAlt = x.PictureAlt,
                    PictureTitle = x.PictureTitle,
                    ShortDescription = x.ShortDescription,
                    Slug = x.Slug,
                    TimeToRead = x.TimeToRead,
                    Title = x.Title,
                    ViewedTime = x.ViewedTime
                }).ToListAsync(),
                    Status = ArticleStatus.Success
                };
            }
            catch (Exception ex)
            {
                await File.AppendAllTextAsync(
                                                        Path.Combine("wwwroot", "Logs.txt"), ex.ToString());

                return new ArticleResult
                {
                    Result = new { },
                    Status = ArticleStatus.Error
                };
            }
        });

        public async Task<List<string>> GetAllKeywords()
        => await Task.Run(async () =>
        {
            return _context.Articles.Select(x => x.Keywords)
            .ToList();
        });

        public async Task<ArticleResult> GetBySlug(string slug, bool addViewTime = false)
        => await Task.Run(async () =>
        {
            try
            {
                var article = await _context.Articles.FirstOrDefaultAsync(x=>x.Slug == slug);

                return new ArticleResult
                {
                    Result = new ArticleDto
                    {
                        Title = article.Title,
                        CategoryId = article.CategoryId,
                        Content = article.Content,
                        CreationDate = article.CreationDate.ToPersianDate(),
                        Id = article.Id,
                        Keywords = article.Keywords,
                        Picture = article.Picture,
                        PictureAlt = article.PictureAlt,
                        PictureTitle = article.PictureTitle,
                        ShortDescription = article.ShortDescription,
                        Slug = article.Slug,
                        TimeToRead = article.TimeToRead,
                        ViewedTime = article.ViewedTime + 1
                    },
                    Status = ArticleStatus.Success
                };
            }
            catch (Exception ex)
            {
                await File.AppendAllTextAsync(
                                        Path.Combine("wwwroot", "Logs.txt"), ex.ToString());

                return new ArticleResult
                {
                    Result = new { },
                    Status = ArticleStatus.Error
                };
            }
        });

        public async Task<ArticleResult> GetLatestArticles(int skip, int take)
        => await Task.Run(async () =>
        {
            try
            {
                return new ArticleResult
                {
                    Result = await _context.Articles
                .Skip(skip)
                .Take(take)
                .Select(x => new ArticleDto
                {
                    Id = x.Id,
                    CategoryId = x.CategoryId,
                    Content = x.Content,
                    CreationDate = x.CreationDate.ToPersianDate(),
                    Keywords = x.Keywords,
                    Picture = x.Picture,
                    PictureAlt = x.PictureAlt,
                    PictureTitle = x.PictureTitle,
                    ShortDescription = x.ShortDescription,
                    Slug = x.Slug,
                    TimeToRead = x.TimeToRead,
                    Title = x.Title,
                    ViewedTime = x.ViewedTime
                }).OrderByDescending(x => x.CreationDate).ToListAsync(),
                    Status = ArticleStatus.Success
                };
            }
            catch (Exception ex)
            {
                await File.AppendAllTextAsync(
                                                        Path.Combine("wwwroot", "Logs.txt"), ex.ToString());

                return new ArticleResult
                {
                    Result = new { },
                    Status = ArticleStatus.Error
                };
            }
        });

        public async Task<bool> IsExists(Expression<Func<Article, bool>> expression)
        => await Task.Run(async () =>
        {
            return await _context.Articles.AnyAsync(expression);
        });

        public async Task<ArticleResult> Search(ArticleSearchModel searchModel)
        => await Task.Run(async () =>
        {
            try
            {
                var articlesQuery = await _context.Articles
                .Select(x => new ArticleDto
                {
                    Id = x.Id,
                    CategoryId = x.CategoryId,
                    Content = x.Content,
                    CreationDate = x.CreationDate.ToPersianDate(),
                    Keywords = x.Keywords,
                    Picture = x.Picture,
                    PictureAlt = x.PictureAlt,
                    PictureTitle = x.PictureTitle,
                    ShortDescription = x.ShortDescription,
                    Slug = x.Slug,
                    TimeToRead = x.TimeToRead,
                    Title = x.Title,
                    ViewedTime = x.ViewedTime
                }).ToListAsync();

                var articles = new ArticleResult
                {
                    Result = articlesQuery,
                    Status = ArticleStatus.Success
                };

                if (!string.IsNullOrWhiteSpace(searchModel.Title))
                {
                    articles = new ArticleResult
                    {
                        Result = articlesQuery.Where(x => x.Title.ToLower()
                        .Contains(searchModel.Title.ToLower())),
                        Status = ArticleStatus.Success
                    };
                }

                if (searchModel.CategoryId != null)
                {
                    articles = new ArticleResult
                    {
                        Result = articlesQuery.Where(x => x.CategoryId == searchModel.CategoryId),
                        Status = ArticleStatus.Success
                    };
                }

                if (!string.IsNullOrWhiteSpace(searchModel.Keywords))
                {
                    articles = new ArticleResult
                    {
                        Result = articlesQuery.Where(x => x.Keywords
                        .Contains(searchModel.Keywords)),
                        Status = ArticleStatus.Success
                    };
                }

                return articles;
            }
            catch (Exception ex)
            {
                await File.AppendAllTextAsync(
                                                        Path.Combine("wwwroot", "Logs.txt"), ex.ToString());

                return new ArticleResult
                {
                    Result = new { },
                    Status = ArticleStatus.Error
                };
            }
        });
    }
}
