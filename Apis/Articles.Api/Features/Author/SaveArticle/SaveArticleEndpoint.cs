using Articles.Models.Feature.Articles.SaveArticle;
using static Articles.Helper.Auth.Policies.Author;

namespace Articles.Api.Features.Author.SaveArticle;
public class SaveArticleEndpoint : EndpointWithMapping<SaveArticleRequest, SaveArticleResponse, ArticleEntity>
{
    public override void Configure()
    {
        Post("/articles/save-article");
        Policies(AuthorSaveArticle);
    }

    public override async Task HandleAsync(SaveArticleRequest request, CancellationToken cancellationToken)
    {
        Logger.LogDebug("Saving article, ArticleRequest {request}", request);

        var article = await MapToEntityAsync(request);
        var savedArticle = await article.Save(cancellationToken);

        Logger.LogDebug("Saved article {savedArticle}", savedArticle);

        if (savedArticle is null)
        {
            ThrowError("Author is not the article's owner!");
        }

        Response = MapFromEntity(savedArticle!);

        if (Response.Id == 0)
        {
            ThrowError("Unable to save the article!");
        }

        await SendAsync(Response, cancellation: cancellationToken);
    }

    public override Task<ArticleEntity> MapToEntityAsync(SaveArticleRequest request)
    {
        var sp = Resolve<IServiceProvider>();
        var article = new ArticleEntity(sp)
        {
            Id = request.Id.GetValueOrDefault(),
            AuthorId = request.AuthorId,
            Title = request.Title,
            Content = request.Content
        };
        return Task.FromResult(article);
    }
    public override SaveArticleResponse MapFromEntity(ArticleEntity entity)
    {
        return new SaveArticleResponse
        {
            Id = entity.Id,
        };
    }
}