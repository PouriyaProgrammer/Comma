namespace Application.DTOs.ArticleCategory
{
    public class ArticleCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public string PictureAlt { get; set; }
        public string PictureTitle { get; set; }
        public string ShortDescription { get; set; }
        public int CountOfArticles { get; set; }
        public string Slug { get; set; }
        public string Keywords { get; set; }
        public string CreationDate { get; set; }
    }
}
