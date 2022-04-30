using Articles.Database.Entities;
using Articles.Test.Helper.Bases;
using AutoFixture;

namespace Articles.Database.Tests.Entities.ArticleEntityTests;
public class SaveArticleEntityTester :
    BaseEntityTester<SaveArticleEntityServiceCollectionFixture, ArticleEntity>
{
    public SaveArticleEntityTester(
        SaveArticleEntityServiceCollectionFixture spFixture) :
        base(spFixture, new ArticleEntity(spFixture.ServiceProvider))
    {
    }

    protected override void PreEntityBuilder()
    {
        _fixture.Register(() => _fixture
           .Build<CommentEntity>()
           .Without(x => x.Article)
           .Create());
    }

    protected override void ResetDb()
    {
        var comments = _context.Comments.Where(x => x.Id >= 0).ToList();

        if (comments.Count == 0)
        {
            return;
        }

        _context.RemoveRange(comments);
        base.ResetDb();
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
            .With(entity => entity.AuthorId, 20)
            .Create();

        var saved = await sut.Save(default);

        sut.AuthorId = 21;

        var ret = await sut.Save(default);

        ret.ShouldBeNull();
    }

    [Fact]
    public async Task Add_Article_Success()
    {
        var sut = _entityBuilder
            .Without(entity => entity.Id)
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
            .Create();

        var saved = await sut.Save(default);

        sut.Title = "Testtestestest";

        await Task.Delay(1_000);
        var ret = await sut.Save(default);

        ret.ShouldNotBeNull();
        ret.Id.ShouldBeGreaterThan(0);
        ret.Title.ShouldBe("Testtestestest");
        ret.LastModifyedOn.ShouldBeGreaterThan(ret.CreatedOn);

    }
}