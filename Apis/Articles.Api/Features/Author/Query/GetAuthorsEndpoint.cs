using Articles.Database.View;
using Articles.Models.Feature.Author.Query;
using System.Collections.Generic;
using static Articles.Helper.Auth.Policies.Author;

namespace Articles.Api.Features.Author.Query;
public class GetAuthorsEndpoint : EndpointWithMapping<AuthorLookUpRequest, List<AuthorLookUpResponse>, List<AuthorView>>
{
    public override void Configure()
    {
        Get("/authors/get");
        // Policies(Read);
        AllowAnonymous();
    }

    public override async Task HandleAsync(AuthorLookUpRequest request, CancellationToken cancellationToken)
    {
        Logger.LogDebug("Getting author lookup");
        var sp = Resolve<IServiceProvider>();
        var av = new AuthorView(sp);
        var authors = await av.GetAuthors(cancellationToken);
        Logger.LogDebug("Mapping author lookup");
        _ = MapFromEntity(authors);

        await SendAsync(Response, cancellation: cancellationToken);
    }

    public override List<AuthorLookUpResponse> MapFromEntity(List<AuthorView> authors)
    {
        // var resp = new List<AuthorLookUpResponse>();
        foreach (var author in authors)
        {
            Response.Add(new AuthorLookUpResponse
            {
                Id = author.Id,
                FirstName = author.FirstName,
                LastName = author.LastName,
            });
        }
        return Response;
    }
}