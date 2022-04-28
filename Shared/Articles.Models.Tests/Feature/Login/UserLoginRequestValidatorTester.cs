using Articles.Models.Feature.Login;
using FluentValidation.TestHelper;
using Xunit;

namespace Articles.Models.Tests.Feature.Login;
public class UserLoginRequestValidatorTester
{
    private UserLoginRequestValidator _validator;


    public UserLoginRequestValidatorTester()
    {
        _validator = new UserLoginRequestValidator();
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData("", "")]
    public void Should_have_error_when_Email_and_Password_are_not_suplied(
        string? email,
        string? password)
    {
        var model = new UserLoginRequest
        {
            Email = email!,
            Password = password!
        };
        var result = _validator.TestValidate(model);
        result
            .ShouldHaveValidationErrorFor(person => person.Email)
            .WithErrorMessage("Username is required!");
        result
            .ShouldHaveValidationErrorFor(person => person.Password)
            .WithErrorMessage("Password is required!");
    }

    [Theory]
    [InlineData("email", "01234")]
    [InlineData("email.email", "01234567890")]
    public void Should_have_error_when_Email_and_Password_are_invalid(
        string email,
        string password)
    {
        var model = new UserLoginRequest
        {
            Email = email!,
            Password = password!
        };
        var result = _validator.TestValidate(model);
        result
            .ShouldHaveValidationErrorFor(person => person.Email)
            .WithErrorMessage("Please include an '@' in the email address.");
        result
            .ShouldHaveValidationErrorFor(person => person.Password)
            .WithErrorMessage("Password length must be between 6 and 10 characters.");
    }

    [Theory]
    [InlineData("e@e", "012345")]
    [InlineData("e@e.com", "0123456789")]
    public void Should_not_have_error_when_Email_and_Password_are_valid(
        string email,
        string password)
    {
        var model = new UserLoginRequest
        {
            Email = email!,
            Password = password!
        };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(person => person.Email);
        result.ShouldNotHaveValidationErrorFor(person => person.Password);
    }
}