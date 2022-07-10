namespace Application.DTOs.Article
{
    public class CreateArticle
    {
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Content { get; set; }
        public IFormFile? Picture { get; set; }
        public string PictureAlt { get; set; }
        public string PictureTitle { get; set; }
        public string TimeToRead { get; set; }
        public string Keywords { get; set; }
        public string Slug { get; set; }
    }
}
