using Articles.Models.Feature.Articles.SaveArticle;
using Articles.Test.Helper.Bases;

namespace Articles.Test.Helper.TheoryData;

public class PendingApprovalArticlesNoParamTheoryData :
    TheoryData<PendingApprovalArticleRequest>
{
    public PendingApprovalArticlesNoParamTheoryData()
    {
        Add(new PendingApprovalArticleRequest());
    }
}
public class PendingApprovalArticlesInvalidParamTheoryData :
TheoryData<PendingApprovalArticleRequest>
{
    public PendingApprovalArticlesInvalidParamTheoryData()
    {
        Add(new PendingApprovalArticleRequest { StartDate = DateTimeOffset.UtcNow });
        Add(new PendingApprovalArticleRequest { EndDate = DateTimeOffset.UtcNow });
        Add(new PendingApprovalArticleRequest
        {
            StartDate = DateTimeOffset.UtcNow.AddDays(10),
            EndDate = DateTimeOffset.UtcNow
        });
    }
}

public class PendingApprovalArticlesValidParamTheoryData :
    TheoryData<PendingApprovalArticleRequest>
{
    public PendingApprovalArticlesValidParamTheoryData()
    {
        Add(new PendingApprovalArticleRequest
        {
            Ids = new[] { 1, 2, 3, 4 }
        });

        Add(new PendingApprovalArticleRequest
        {
            StartDate = DateTimeOffset.UtcNow,
            EndDate = DateTimeOffset.UtcNow.AddDays(10)
        });

        Add(new PendingApprovalArticleRequest
        {
            Ids = new[] { 1, 2, 3, 4 },
            StartDate = DateTimeOffset.UtcNow,
            EndDate = DateTimeOffset.UtcNow.AddDays(10)
        });
    }
}

public record PendingApprovalArticlesModelData(
    PendingApprovalArticleRequest Data,
    int Total)
{ }
public class PendingApprovalArticlesValidQueryParamTheoryData :
    TheoryData<PendingApprovalArticlesModelData>
{
    public PendingApprovalArticlesValidQueryParamTheoryData()
    {
        Add(new PendingApprovalArticlesModelData(
            new PendingApprovalArticleRequest
            {
                Ids = new[] { 1 }
            },
            0));

        Add(new PendingApprovalArticlesModelData(
            new PendingApprovalArticleRequest
            {
                Ids = new[] { 1, 2, 3, 4 }
            },
            3));

        Add(new PendingApprovalArticlesModelData(
            new PendingApprovalArticleRequest
            {
                StartDate = DateTimeOffset.UtcNow.AddDays(-5),
                EndDate = DateTimeOffset.UtcNow
            },
            2));
        Add(new PendingApprovalArticlesModelData(
            new PendingApprovalArticleRequest
            {
                Ids = new[] { 3 },
                StartDate = DateTimeOffset.UtcNow.AddDays(-10),
                EndDate = DateTimeOffset.UtcNow
            },
            3));
    }
}