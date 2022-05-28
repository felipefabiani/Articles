using FluentValidation;
using System;
using System.Collections.Generic;

namespace Articles.Models.Feature.Articles.SaveArticle
{
    public class PenddingApprovalArticlesRequest
    {
        public int[]? Ids { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
    }

    public class PenddingApprovalArticlesValidator : AbstractValidator<PenddingApprovalArticlesRequest>
    {
        public PenddingApprovalArticlesValidator()
        {
            RuleFor(m => m.StartDate)
                .NotEmpty()
                    .WithMessage("Start Date is Required")
                    .When(m => m.EndDate.HasValue);                

            RuleFor(x => x.EndDate)
                .NotEmpty()
                    .WithMessage("End Date is Required")
                    .When(m => m.StartDate.HasValue)
                .GreaterThanOrEqualTo(x => x.StartDate)
                    .WithMessage("End date must after Start date")
                    .When(x => x.StartDate.HasValue);
        }
    }

    public class PenddingApprovalArticleResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTimeOffset CreatedOn { get; set; }
        public string AuthorName { get; set; } = string.Empty;
    }
}