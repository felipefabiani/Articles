using Articles.Models.Feature.Articles.SaveArticle;

namespace Articles.Api.Features.Author.SaveArticle;

public class SaveArticleMapper : Mapper<SaveArticleRequest, SaveArticleResponse, ArticleEntity>
{
    public override Task<ArticleEntity> ToEntityAsync(SaveArticleRequest request)
    {
        var article = Resolve<ArticleEntity>();

        article.Id = request.Id.GetValueOrDefault();
        article.AuthorId = request.AuthorId;
        article.Title = request.Title;
        article.Content = request.Content;
        return Task.FromResult(article);
    }

    public override SaveArticleResponse FromEntity(ArticleEntity e)
    {
        return new SaveArticleResponse
        {
            Id = e.Id,
        };
    }
}