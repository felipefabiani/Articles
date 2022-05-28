using Articles.Models.Feature.Articles.SaveArticle;
using Articles.Test.Helper.Bases;

namespace Articles.Test.Helper.TheoryData;

public class PenddingApprovalArticlesInvalidParamTheoryData :
    TheoryData<PenddingApprovalArticlesRequest>
{
    public PenddingApprovalArticlesInvalidParamTheoryData()
    {
        Add(new PenddingApprovalArticlesRequest { StartDate = DateTimeOffset.UtcNow });
        Add(new PenddingApprovalArticlesRequest { EndDate = DateTimeOffset.UtcNow });
        Add(new PenddingApprovalArticlesRequest
        {
            StartDate = DateTimeOffset.UtcNow.AddDays(10),
            EndDate = DateTimeOffset.UtcNow
        });
    }
}

public class PenddingApprovalArticlesValidParamTheoryData :
    TheoryData<PenddingApprovalArticlesRequest>
{
    public PenddingApprovalArticlesValidParamTheoryData()
    {
        Add(new PenddingApprovalArticlesRequest());
        Add(new PenddingApprovalArticlesRequest
        {
            StartDate = DateTimeOffset.UtcNow,
            EndDate = DateTimeOffset.UtcNow.AddDays(10)
        });

        Add(new PenddingApprovalArticlesRequest
        {
            Ids = new[] { 1, 2, 3, 4 },
            StartDate = DateTimeOffset.UtcNow,
            EndDate = DateTimeOffset.UtcNow.AddDays(10)
        });
    }
}