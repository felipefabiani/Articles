using Articles.Models.Feature.Articles.SaveArticle;
using Articles.Test.Helper.TheoryData;
using AutoFixture;
using FluentValidation.TestHelper;
using Xunit;

namespace Articles.Models.Tests.Feature.Articles.SaveArticle;
public class SaveArticleValidatorTester
{
    private SaveArticleValidator _validator;

    public SaveArticleValidatorTester()
    {
        _validator = new SaveArticleValidator();
    }

    [Theory]
    [ClassData(typeof(SaveArticleInvalidParamTheoryData))]
    public void Should_have_error_when_requeired_fields_are_not_suplied(SaveArticleTheoryModel data)
    {
        var model = new SaveArticleRequest
        {
            Title = data.Title!,
            Content = data.Content!,
            AuthorId = data.AuthorId
        };
        var result = _validator.TestValidate(model);
        result
            .ShouldHaveValidationErrorFor(m => m.Title)
            .WithErrorMessage(data.TitleMessage);

        result
            .ShouldHaveValidationErrorFor(m => m.Content)
            .WithErrorMessage(data.ContentMessage);

        result
            .ShouldHaveValidationErrorFor(m => m.AuthorId)
            .WithErrorMessage(data.AuthorMessage);
    }

    [Theory]
    [ClassData(typeof(SaveArticleValidParamTheoryData))]
    public void Should_have_error_when_requeired_fields_are_valid(SaveArticleTheoryModel data)
    {
        var fixture = new Fixture();
        var model = fixture.Build<SaveArticleRequest>()
            .With(x => x.Title, data.Title)
            .With(x => x.Content, data.Content)
            .With(x => x.AuthorId, data.AuthorId)
            .Create();

        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(m => m.Title);
        result.ShouldNotHaveValidationErrorFor(m => m.Content);
        result.ShouldNotHaveValidationErrorFor(m => m.AuthorId);
    }
}