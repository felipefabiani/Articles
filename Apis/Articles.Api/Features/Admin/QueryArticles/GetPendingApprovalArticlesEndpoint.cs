﻿using Articles.Models.Feature.Articles.Query;
using System.Collections.Generic;

namespace Articles.Api.Features.Admin.QueryArticles;
public class GetPendingApprovalArticlesEndpoint : EndpointWithMapping<PendingApprovalArticleRequest, List<PendingApprovalArticleResponse>, List<ArticleEntity>>
{
    public override void Configure()
    {
        Get("/articles/getPendingApproval");
        // Policies(AuthorSaveArticle);

    }

    public override async Task HandleAsync(PendingApprovalArticleRequest request, CancellationToken cancellationToken)
    {
        Logger.LogDebug("Get Pending Approval Article, request {request}", request);

        var articleEntity = Resolve<ArticleEntity>();
        var pendingApprovalAritcles = await articleEntity
            .GetPendingApprovals(request, cancellationToken)
            .ConfigureAwait(false);

        Response = MapFromEntity(pendingApprovalAritcles);
        await SendAsync(Response, cancellation: cancellationToken);
    }

    public override List<PendingApprovalArticleResponse> MapFromEntity(List<ArticleEntity> e)
    {
        Parallel.ForEach(e, article =>
        {
            Response.Add(new PendingApprovalArticleResponse
            {
                Id = article.Id,
                Title = article.Title,
                Content = article.Content,
                AuthorName = $"{article.Author.FirstName} {article.Author.LastName}",
                CreatedOn = article.CreatedOn
            });
        });

        return Response;
    }
}