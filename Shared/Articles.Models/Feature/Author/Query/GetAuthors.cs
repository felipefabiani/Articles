using FluentValidation;

namespace Articles.Models.Feature.Author.Query
{
    public class AuthorLookUpRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
    }

    public class AuthorLookUpRequestValidator : AbstractValidator<AuthorLookUpRequest>
    {
        public AuthorLookUpRequestValidator()
        {
        }
    }

    public class AuthorLookUpResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName { get => $"{FirstName} {LastName}"; }

        public override string ToString() => FullName;
    }
}