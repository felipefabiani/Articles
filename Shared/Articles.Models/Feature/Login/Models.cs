using Articles.Models.Auth;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models.Feature.Login
{
    public class UserLoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class UserLoginRequestValidator :
         // FluentValueValidator<UserLoginRequest>
         AbstractValidator<UserLoginRequest>
    {
        public UserLoginRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Username is required!")
                .EmailAddress()
                .WithMessage("Invalid email format!");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required!")
                .MinimumLength(6)
                .WithMessage("Password length must be between 6 and 10 characters!")
                .MaximumLength(10)
                .WithMessage("Password length must be between 6 and 10 characters!");
        }
        public Func<object, string, Task<IEnumerable<string>>> ValidateValue1 => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<UserLoginRequest>.CreateWithOptions((UserLoginRequest)model, x => x.IncludeProperties(propertyName)));
            return
                result.IsValid ?
                (IEnumerable<string>)Array.Empty<string>() :
                result.Errors.Select(e => e.ErrorMessage);
        };
    }

    public class UserLoginResponse
    {
        public string FullName { get; set; } = string.Empty;
        public IEnumerable<string> UserRoles { get; set; } = new List<string>();
        public IEnumerable<string> UserClaims { get; set; } = new List<string>();
        public JwtToken Token { get; set; } = new JwtToken();
        public virtual bool HasToken { get => !string.IsNullOrEmpty(Token.Value); }
    }

    public class NullUserLoginResponse : UserLoginResponse
    {
        public override bool HasToken { get => false; }
        public static NullUserLoginResponse Empty => new NullUserLoginResponse();
    }

    public abstract class FluentValueValidator<T> : AbstractValidator<T>
    {

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<T>.CreateWithOptions((T)model, x => x.IncludeProperties(propertyName)));
            return 
                result.IsValid ? 
                (IEnumerable<string>)Array.Empty<string>() : 
                result.Errors.Select(e => e.ErrorMessage);
        };
    }
}

