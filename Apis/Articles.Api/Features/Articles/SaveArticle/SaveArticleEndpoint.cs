using Articles.Api.Infrastructure.Auth;
using Articles.Models.Feature.Articles.SaveArticle;

namespace Articles.Api.Features.Articles.SaveArticle;

public class SaveArticleEndpoint : EndpointWithMapping<SaveArticleRequest, SaveArticleResponse, ArticleEntity>
{
    public override void Configure()
    {
        Post("/author/articles/save-article");
        Claims(RolePermissions.Author);
        Permissions(ClaimPermissions.Article_Save_Own);
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
        var article = Resolve<ArticleEntity>();

        article.Id = request.Id.GetValueOrDefault();
        article.AuthorId = request.AuthorId;
        article.Title = request.Title;
        article.Content = request.Content;
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