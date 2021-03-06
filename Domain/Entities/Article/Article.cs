namespace Domain.Entities.Article
{
    public class Article : EntityBase
    {
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Content { get; set; }
        public string Picture { get; set; }
        public string PictureAlt { get; set; }
        public string PictureTitle { get; set; }
        public int ViewedTime { get; set; }
        public string TimeToRead { get; set; }
        public string Keywords { get; set; }
        public string Slug { get; set; }
        public int CategoryId { get; set; }

        public ArticleCategory.ArticleCategory ArticleCategory { get; set; }
    }
}
