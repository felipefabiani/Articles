using FluentValidation.TestHelper;
namespace Articles.Api.Test.Features.Login;
public class UserLoginRequestValidatorTest
{
    private UserLoginRequestValidator _validator = null!;

    public UserLoginRequestValidatorTest()
    {
        _validator = new UserLoginRequestValidator();

    }

    [Theory]
    [InlineData(null, "NotEmptyValidator", null, "NotEmptyValidator" )]
    [InlineData("", "NotEmptyValidator", "", "NotEmptyValidator" )]
    [InlineData("invalid-email", "EmailValidator", "", "NotEmptyValidator" )]
    [InlineData("invalid-email", "EmailValidator", "1234", "MinimumLengthValidator")]
    [InlineData("invalid-email", "EmailValidator", "01234567891", "MaximumLengthValidator")]
    public void Should_have_error_when_Name_is_null(
        string email,
        string emailErrorCode,
        string password,
        string passwordErrorCode)
    {
        var model = new UserLoginRequest { Email = email, Password = password };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(person => person.Email).WithErrorCode(emailErrorCode);
        result.ShouldHaveValidationErrorFor(person => person.Password).WithErrorCode(passwordErrorCode);
    }

    [Theory]
    [InlineData("t@t.com", "012345")]
    [InlineData("admin.test@article.ie", "0123456")]
    [InlineData("valid-email@gmail.com", "01234567")]
    [InlineData("new_email@cwb.com", "012345678")]
    [InlineData("l@l.ie", "0123456789")]
    public void Should_not_have_error_when_name_is_specified(
        string email,
        string password)
    {
        var model = new UserLoginRequest { Email = email, Password = password };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(person => person.Email);
        result.ShouldNotHaveValidationErrorFor(person => person.Password);
    }
}