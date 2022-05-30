using Articles.Database.Entities;
using Articles.Test.Helper.Bases;
using Articles.Test.Helper.Fixture;
using Articles.Test.Helper.TheoryData;
using AutoFixture;

namespace Articles.Database.Tests.Entities.ArticleEntityTests;
public class GetPendingApprovalSaveArticleEntityTester :
    ContextDb<ServiceCollectionFixture, ArticleEntity>
{
    public GetPendingApprovalSaveArticleEntityTester(
        ServiceCollectionFixture spFixture) :
        base(spFixture)
    {
    }

    [Theory]
    [ClassData(typeof(PendingApprovalArticlesValidQueryParamTheoryData))]
    public async Task Add_Article_required_fields_not_supplied_Fail(
        PendingApprovalArticlesModelData data)
    {
        ArticleSeed();
        var sut = _entityBuilder
           .OmitAutoProperties()
           .Create();

        var ret = await sut.GetPendingApprovals(data.Data).ConfigureAwait(false);
        ret.Count.ShouldBe(data.Total);
    }
}