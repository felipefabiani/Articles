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
        var saved = _entityBuilder
            .With(entity => entity.Id, 0)
            .With(entity => entity.AuthorId, 1)
            .Without(entity => entity.Comments)
            .Without(entity => entity.Author)
            .Create();

        _ = await saved.Save(default);

        var sut = _entityBuilder
           .With(entity => entity.Id, saved.Id)
           .With(entity => entity.AuthorId, saved.AuthorId + 1)
           .Without(entity => entity.Comments)
           .Without(entity => entity.Author)
           .Create();

        var ret = await sut.Save(default);

        ret.ShouldBeNull();
    }

    [Fact]
    public async Task Add_Article_Success()
    {
        var sut = _entityBuilder
        .With(entity => entity.Id, 0)
        .With(entity => entity.AuthorId, 1)
        .Without(entity => entity.Comments)
        .Without(entity => entity.Author)
        .Create();

        var ret = await sut.Save(default);

        ret.ShouldNotBeNull();
        ret.Id.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task Update_Article_Success()
    {
        var sut = _entityBuilder
           .With(entity => entity.Id, 0)
           .With(entity => entity.AuthorId, 1)
           .Without(entity => entity.Comments)
           .Without(entity => entity.Author)
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