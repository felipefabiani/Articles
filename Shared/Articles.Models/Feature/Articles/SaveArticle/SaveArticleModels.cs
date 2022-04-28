using FluentValidation;

namespace Articles.Models.Feature.Articles.SaveArticle
{
    public class SaveArticleRequest : ArticleModel
    {
        public int AuthorID { get; set; }
    }

    public class SaveArticleValidator : AbstractValidator<SaveArticleRequest>
    {
        public SaveArticleValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required!")
                .MinimumLength(10).WithMessage("Title is too short!");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required!")
                .MinimumLength(10).WithMessage("Content is too short!");

            RuleFor(x => x.AuthorID)
                .GreaterThan(0).WithMessage("AuthorID is required!");
        }
    }

    public class SaveArticleResponse
    {
        public string Message => "Article saved!";
        public string? ArticleID { get; set; }
    }
}