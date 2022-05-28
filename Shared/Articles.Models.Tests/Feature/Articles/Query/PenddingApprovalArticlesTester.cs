using Articles.Models.Feature.Articles.SaveArticle;
using Articles.Test.Helper.TheoryData;
using FluentValidation.TestHelper;
using Xunit;

namespace Articles.Models.Tests.Feature.Articles.SaveArticle;
public class PenddingApprovalArticlesTester
{
    private PenddingApprovalArticlesValidator _validator;

    public PenddingApprovalArticlesTester()
    {
        _validator = new PenddingApprovalArticlesValidator();
    }

    [Theory]
    [ClassData(typeof(PenddingApprovalArticlesNoParamTheoryData))]
    public void Should_have_error_when_no_parameter(
        PenddingApprovalArticlesRequest data)
    {
        var result = _validator.TestValidate(data);

        result.ShouldHaveValidationErrorFor(x => x.Ids).WithErrorCode("NotEmptyValidator");
        result.ShouldHaveValidationErrorFor(x => x.StartDate).WithErrorCode("NotEmptyValidator");
    }
    
    [Theory]
    [ClassData(typeof(PenddingApprovalArticlesInvalidParamTheoryData))]
    public void Should_have_errors_when_invalids_dates_parameters(
        PenddingApprovalArticlesRequest data)
    {
        var result = _validator.TestValidate(data);

        switch (data)
        {
            case PenddingApprovalArticlesRequest x when x.StartDate.HasValue && x.EndDate.HasValue:
                result.ShouldHaveValidationErrorFor(m => m.EndDate)
                    .WithErrorCode("GreaterThanOrEqualValidator");
                break;
            case PenddingApprovalArticlesRequest x when x.StartDate.HasValue:
                result.ShouldHaveValidationErrorFor(m => m.EndDate)
                    .WithErrorCode("NotEmptyValidator");
                break;
            case PenddingApprovalArticlesRequest x when x.EndDate.HasValue:
                result.ShouldHaveValidationErrorFor(m => m.StartDate)
                    .WithErrorCode("NotEmptyValidator");
                break;
        }
    }

    [Theory]
    [ClassData(typeof(PenddingApprovalArticlesValidParamTheoryData))]
    public void Should_have_no_error_when_requeired_fields_are_valid(
        PenddingApprovalArticlesRequest data)
    {
        var result = _validator.TestValidate(data);
        result.ShouldNotHaveValidationErrorFor(m => m.Ids);
        result.ShouldNotHaveValidationErrorFor(m => m.StartDate);
        result.ShouldNotHaveValidationErrorFor(m => m.EndDate);
    }
}