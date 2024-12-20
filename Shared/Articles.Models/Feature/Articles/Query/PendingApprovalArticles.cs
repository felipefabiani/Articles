﻿using FluentValidation;
using System;

namespace Articles.Models.Feature.Articles.Query
{
    public class PendingApprovalArticleRequest
    {
        public int[]? Ids { get; set; }
        public int Id { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
    }

    public class PendingApprovalArticlesValidator : AbstractValidator<PendingApprovalArticleRequest>
    {
        public PendingApprovalArticlesValidator()
        {
            //RuleFor(x => x.Id)
            // .NotEmpty().WithMessage("Author or Period is requeired")
            // .Unless(x => x.StartDate.HasValue || x.EndDate.HasValue);

            //RuleFor(m => m.StartDate)
            //    .NotEmpty()
            //        .WithMessage("Author or Period is requeired")
            //        .Unless(m => m.Id > 0 || m.EndDate.HasValue);

            //RuleFor(m => m.StartDate)
            //    .NotEmpty()
            //        .WithMessage("Start date is requeired")
            //        .When(x => x.EndDate.HasValue);

            //RuleFor(x => x.EndDate)
            //    .NotEmpty()
            //        .WithMessage("End Date is Required")
            //        .Unless(m => m.Ids?.Length > 0 || m.StartDate.HasValue);

            //RuleFor(x => x.EndDate)
            //    .NotEmpty()
            //        .WithMessage("End Date is Required")
            //        .When(m => m.StartDate.HasValue)
            //    .GreaterThanOrEqualTo(x => x.StartDate)
            //        .WithMessage("End date must after Start date")
            //        .When(x => x.StartDate.HasValue);
        }
    }

    public class PendingApprovalArticleResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTimeOffset CreatedOn { get; set; }
        public string AuthorName { get; set; } = string.Empty;
    }
}