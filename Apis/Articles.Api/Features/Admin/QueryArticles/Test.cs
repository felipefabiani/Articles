using Articles.Models.Feature.Articles.SaveArticle;
using System.Collections.Generic;
using static Articles.Helper.Auth.Policies.Author;

namespace Articles.Api.Features.Admin.SaveArticle;


public class Item
{
    public int Id { get; set; }
}
public class MyRequest
{
    public IEnumerable<Item> Items { get; set; }
    public int[] Codes { get; set; }
}
public class TestEndpoint : Endpoint<MyRequest>
{
    public override void Configure()
    {
        Get("/my-endpoint");
        // Policies(AuthorSaveArticle);
    }

    public override async Task HandleAsync(MyRequest request, CancellationToken cancellationToken)
    {
        Logger.LogDebug("Get Pending Approval Article, request {request}", request);

        var queryParam = QueryTest<int[]>("Codes");
        var items = QueryTest<IEnumerable<Item>>("items");
        
        await SendAsync(Response, cancellation: cancellationToken);
    }



    protected T? QueryTest<T>(string paramName, bool isRequired = true)
    {
        if (HttpContext.Request.Query.TryGetValue(paramName, out var val))
        {
            //var res = typeof(T).ValueParser()?.Invoke(val);

            //if (res?.isSuccess is true)
            //    return (T?)res?.value;

            //if (isRequired)
            //    ValidationFailures.Add(new(paramName, "Unable to read value of query parameter!"));
        }
        else if (isRequired)
        {
            ValidationFailures.Add(new(paramName, "Query parameter was not found!"));
        }

        ThrowIfAnyErrors();

        return default;// not required and retrieval failed
    }
}