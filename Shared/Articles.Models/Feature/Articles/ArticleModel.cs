namespace Articles.Models.Feature.Articles
{
    public class ArticleModel
    {
        public int? Id { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
    }
}