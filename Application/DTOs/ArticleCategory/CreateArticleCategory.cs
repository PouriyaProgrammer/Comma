namespace Application.DTOs.ArticleCategory
{
    public class CreateArticleCategory
    {
        public string Name { get; set; }
        public IFormFile? Picture { get; set; }
        public string PictureAlt { get; set; }
        public string PictureTitle { get; set; }
        public string ShortDescription { get; set; }
        public string Slug { get; set; }
        public string Keywords { get; set; }
    }
}
