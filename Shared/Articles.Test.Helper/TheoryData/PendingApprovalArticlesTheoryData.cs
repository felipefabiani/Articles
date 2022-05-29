using Articles.Models.Feature.Articles.SaveArticle;
using Articles.Test.Helper.Bases;

namespace Articles.Test.Helper.TheoryData;

public class PendingApprovalArticlesNoParamTheoryData :
    TheoryData<PendingApprovalArticlesRequest>
{
    public PendingApprovalArticlesNoParamTheoryData()
    {
        Add(new PendingApprovalArticlesRequest());
    }
}
public class PendingApprovalArticlesInvalidParamTheoryData :
TheoryData<PendingApprovalArticlesRequest>
{
    public PendingApprovalArticlesInvalidParamTheoryData()
    {
        Add(new PendingApprovalArticlesRequest { StartDate = DateTimeOffset.UtcNow });
        Add(new PendingApprovalArticlesRequest { EndDate = DateTimeOffset.UtcNow });
        Add(new PendingApprovalArticlesRequest
        {
            StartDate = DateTimeOffset.UtcNow.AddDays(10),
            EndDate = DateTimeOffset.UtcNow
        });
    }
}

public class PendingApprovalArticlesValidParamTheoryData :
    TheoryData<PendingApprovalArticlesRequest>
{
    public PendingApprovalArticlesValidParamTheoryData()
    {
        Add(new PendingApprovalArticlesRequest
        {
            Ids = new[] { 1, 2, 3, 4 }
        });

        Add(new PendingApprovalArticlesRequest
        {
            StartDate = DateTimeOffset.UtcNow,
            EndDate = DateTimeOffset.UtcNow.AddDays(10)
        });

        Add(new PendingApprovalArticlesRequest
        {
            Ids = new[] { 1, 2, 3, 4 },
            StartDate = DateTimeOffset.UtcNow,
            EndDate = DateTimeOffset.UtcNow.AddDays(10)
        });
    }
}

public record PendingApprovalArticlesModelData(
    PendingApprovalArticlesRequest Data,
    int Total)
{ }
public class PendingApprovalArticlesValidQueryParamTheoryData :
    TheoryData<PendingApprovalArticlesModelData>
{
    public PendingApprovalArticlesValidQueryParamTheoryData()
    {
        Add(new PendingApprovalArticlesModelData(
            new PendingApprovalArticlesRequest
            {
                Ids = new[] { 1 }
            },
            0));

        Add(new PendingApprovalArticlesModelData(
            new PendingApprovalArticlesRequest
            {
                Ids = new[] { 1, 2, 3, 4 }
            },
            3));

        Add(new PendingApprovalArticlesModelData(
            new PendingApprovalArticlesRequest
            {
                StartDate = DateTimeOffset.UtcNow.AddDays(-5),
                EndDate = DateTimeOffset.UtcNow
            },
            2));
        Add(new PendingApprovalArticlesModelData(
            new PendingApprovalArticlesRequest
            {
                Ids = new[] { 3, },
                StartDate = DateTimeOffset.UtcNow.AddDays(-10),
                EndDate = DateTimeOffset.UtcNow
            },
            3));
    }
}