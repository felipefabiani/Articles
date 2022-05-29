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
        base(spFixture, new ArticleEntity(spFixture.ServiceProvider))
    {
    }

    protected override void SeedDb()
    {
        var articles = _articles.ToList();
        _contextWriteOnly.AddRange(articles);
        _contextWriteOnly.SaveChanges();

        var dates = new[] { -10, -5, -2, -1, -1, -1 };
        for (int i = 0; i < articles.Count; i++)
        {
            articles[i].CreatedOn = articles[i].CreatedOn.AddDays(dates[i]);
        }

        _contextWriteOnly.UpdateRange(articles);
        _contextWriteOnly.SaveChanges();
    }

    [Theory]
    [ClassData(typeof(PendingApprovalArticlesValidQueryParamTheoryData))]
    public async Task Add_Article_required_fields_not_supplied_Fail(
        PendingApprovalArticlesModelData data)
    {
        var sut = _entityBuilder
           .OmitAutoProperties()
           .Create();

        var ret = await sut.GetPendingApprovals(data.Data).ConfigureAwait(false);
        ret.Count().ShouldBe(data.Total);
    }
}