using Articles.Models.Feature.Articles.Query;
using Articles.Test.Helper.TheoryData;
using FluentValidation.TestHelper;
using Xunit;

namespace Articles.Models.Tests.Feature.Articles.Query;
public class PendingApprovalArticlesTester
{
    private PendingApprovalArticlesValidator _validator;

    public PendingApprovalArticlesTester()
    {
        _validator = new PendingApprovalArticlesValidator();
    }

    [Theory]
    [ClassData(typeof(PendingApprovalArticlesNoParamTheoryData))]
    public void Should_have_error_when_no_parameter(
        PendingApprovalArticleRequest data)
    {
        var result = _validator.TestValidate(data);

        result.ShouldHaveValidationErrorFor(x => x.Ids).WithErrorCode("NotEmptyValidator");
        result.ShouldHaveValidationErrorFor(x => x.StartDate).WithErrorCode("NotEmptyValidator");
    }

    [Theory]
    [ClassData(typeof(PendingApprovalArticlesInvalidParamTheoryData))]
    public void Should_have_errors_when_invalids_dates_parameters(
        PendingApprovalArticleRequest data)
    {
        var result = _validator.TestValidate(data);

        switch (data)
        {
            case PendingApprovalArticleRequest x when x.StartDate.HasValue && x.EndDate.HasValue:
                result.ShouldHaveValidationErrorFor(m => m.EndDate).WithErrorCode("GreaterThanOrEqualValidator");
                result.ShouldNotHaveValidationErrorFor(x => x.Ids);
                result.ShouldNotHaveValidationErrorFor(x => x.StartDate);
                break;
            case PendingApprovalArticleRequest x when x.StartDate.HasValue:
                result.ShouldHaveValidationErrorFor(m => m.EndDate).WithErrorCode("NotEmptyValidator");
                result.ShouldNotHaveValidationErrorFor(x => x.Ids);
                result.ShouldNotHaveValidationErrorFor(x => x.StartDate);
                break;
            case PendingApprovalArticleRequest x when x.EndDate.HasValue:
                result.ShouldHaveValidationErrorFor(m => m.StartDate).WithErrorCode("NotEmptyValidator");
                result.ShouldNotHaveValidationErrorFor(x => x.Ids);
                result.ShouldNotHaveValidationErrorFor(x => x.EndDate);
                break;
        }
    }

    [Theory]
    [ClassData(typeof(PendingApprovalArticlesValidParamTheoryData))]
    public void Should_have_no_error_when_requeired_fields_are_valid(
        PendingApprovalArticleRequest data)
    {
        var result = _validator.TestValidate(data);
        result.ShouldNotHaveValidationErrorFor(m => m.Ids);
        result.ShouldNotHaveValidationErrorFor(m => m.StartDate);
        result.ShouldNotHaveValidationErrorFor(m => m.EndDate);
    }
}