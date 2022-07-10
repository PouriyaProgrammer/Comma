namespace Application.DTOs.Article
{
    public class ArticleSearchModel
    {
        public string Title { get; set; }
        public int? CategoryId { get; set; }
        public string Keywords { get; set; }
    }
}
