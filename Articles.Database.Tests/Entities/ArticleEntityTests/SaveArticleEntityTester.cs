using Articles.Database.Entities;
using Articles.Test.Helper.Bases;
using Articles.Test.Helper.Fixture;
using AutoFixture;

namespace Articles.Database.Tests.Entities.ArticleEntityTests;
public class SaveArticleEntityTester :
    ContextDb<ServiceCollectionFixture, ArticleEntity>
{
    public SaveArticleEntityTester(
        ServiceCollectionFixture spFixture) :
        base(spFixture, new ArticleEntity(spFixture.ServiceProvider))
    {
    }

    protected override void PreEntityBuilder()
    {
        _fixture.Register(() => _fixture?
           .Build<CommentEntity>()
           .Without(x => x.Article)
           .Create());

        _fixture.Register(() => _contextWriteOnly.Users.First());
    }

    [Fact]
    public async Task Add_Article_required_fields_not_supplied_Fail()
    {
        var sut = _entityBuilder
           .OmitAutoProperties()
           .Create();

        sut.Title = null!;
        sut.Content = null!;
        sut.FullName = null!;

        var ret = await Should.ThrowAsync<DbUpdateException>(async () => await sut.Save(default));
        ret.Message.ShouldContain("Title");
        ret.Message.ShouldContain("Content");
        ret.Message.ShouldContain("FullName");
    }

    [Fact]
    public async Task Update_Article_authors_doesnt_match_Fail()
    {
        var sut = _entityBuilder
            .Without(entity => entity.Id)
            .With(entity => entity.AuthorId, 1)
            .Create();

        var saved = await sut.Save(default);

        sut.AuthorId += 1;

        var ret = await sut.Save(default);

        ret.ShouldBeNull();
    }

    [Fact]
    public async Task Add_Article_Success()
    {
        var sut = _entityBuilder
           .Without(entity => entity.Id)
           .With(entity => entity.AuthorId, 1)
           .Create();

        var ret = await sut.Save(default);
        ret.ShouldNotBeNull();
        ret.Id.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task Update_Article_Success()
    {
        var sut = _entityBuilder
           .Without(entity => entity.Id)
           .With(entity => entity.AuthorId, 1)
           .Create();

        var saved = await sut.Save(default);

        var title = "Testtestestest";
        sut.Title = title;

        await Task.Delay(1_000);
        var ret = await sut.Save(default);

        ret.ShouldNotBeNull();
        ret.Id.ShouldBeGreaterThan(0);
        ret.Title.ShouldBe(title);
        ret.LastModifyedOn.ShouldBeGreaterThan(ret.CreatedOn);
    }
}