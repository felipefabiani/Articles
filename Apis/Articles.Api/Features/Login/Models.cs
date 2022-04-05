using FluentValidation;

namespace Author.Login;

public class UserLoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class UserLoginRequestValidator : Validator<UserLoginRequest>
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
            .WithMessage("The length of 'Password' must be between 6 and 10 characters!")
            .MaximumLength(10)
            .WithMessage("The length of 'Password' must be between 6 and 10 characters!");
    }
}

public class UserLoginResponse
{
    public string FullName { get; set; } = string.Empty;
    public IEnumerable<string> UserRoles { get; set; } = new List<string>();
    public IEnumerable<string> UserClaims { get; set; } = new List<string>();
    public JwtToken Token { get; set; } = new();
    public virtual bool HasToken { get => !string.IsNullOrEmpty(Token.Value); }
}

public class NullUserLoginResponse : UserLoginResponse
{
    public override bool HasToken { get => false; }
    public static NullUserLoginResponse Empty => new();
}