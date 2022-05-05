using Articles.Models.Feature.Articles.SaveArticle;
using Articles.Test.Helper.Extensions;
using AutoFixture;
using FluentValidation.TestHelper;
using Xunit;

namespace Articles.Models.Tests.Feature.Articles.SaveArticle;
public class SaveArticleValidatorTester
{
    private SaveArticleValidator _validator;
    private Fixture _fixture;

    public SaveArticleValidatorTester()
    {
        _validator = new SaveArticleValidator();
        _fixture = new Fixture();
    }

    [Theory]
    [InlineData(null, null, 0)]
    [InlineData("", "", -1)]
    public void Should_have_error_when_requeired_fields_are_not_suplied(
        string? title,
        string? content,
        int authorId)
    {
        var model = new SaveArticleRequest
        {
            Title = title!,
            Content = content!,
            AuthorId = authorId
        };
        var result = _validator.TestValidate(model);
        result
            .ShouldHaveValidationErrorFor(m => m.Title)
            .WithErrorMessage("Title is required!");

        result
            .ShouldHaveValidationErrorFor(m => m.Content)
            .WithErrorMessage("Content is required!");

        result
            .ShouldHaveValidationErrorFor(m => m.AuthorId)
            .WithErrorMessage("AuthorID is required!");
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(9, 9)]
    public void Should_have_error_when_requeired_fields_are_invalid(
        int titleSize,
        int contentSize)
    {
        var model = _fixture.Build<SaveArticleRequest>()
            .WithStringLength(x => x.Title, titleSize)
            .WithStringLength(x => x.Content, contentSize)
            .Create();

        var result = _validator.TestValidate(model);
        result
            .ShouldHaveValidationErrorFor(m => m.Title)
            .WithErrorMessage("Title is too short!");

        result
            .ShouldHaveValidationErrorFor(m => m.Content)
            .WithErrorMessage("Content is too short!");
    }

    [Theory]
    [InlineData(10, 10, 1)]
    [InlineData(100, 100, 9090)]
    public void Should_have_error_when_requeired_fields_are_valid(
        int titleSize,
        int contentSize,
        int authorId)
    {
        var fixture = new Fixture();
        var model = fixture.Build<SaveArticleRequest>()
            .WithStringLength(x => x.Title, titleSize)
            .WithStringLength(x => x.Content, contentSize)
            .With(x => x.AuthorId, authorId)
            .Create();

        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(m => m.Title);
        result.ShouldNotHaveValidationErrorFor(m => m.Content);
        result.ShouldNotHaveValidationErrorFor(m => m.AuthorId);
    }
}