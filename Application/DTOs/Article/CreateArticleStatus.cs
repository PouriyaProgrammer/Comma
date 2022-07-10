namespace Application.DTOs.Article
{
    public enum CreateArticleStatus
    {
        Success,
        Error,
        ArticleIsExsits,
        PictureNotSave,
        PictureIsNull,
        CategoryNotFound
    }
}
