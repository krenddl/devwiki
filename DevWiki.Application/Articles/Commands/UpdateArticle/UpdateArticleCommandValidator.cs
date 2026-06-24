using FluentValidation;

namespace DevWiki.Application.Articles.Commands.UpdateArticle
{
    public class UpdateArticleCommandValidator : AbstractValidator<UpdateArticleCommand>
    {
        public UpdateArticleCommandValidator()
        {
            RuleFor(v => v.Id)
                .NotEmpty().WithMessage("Article Id is required.");

            RuleFor(v => v.Title)
                .MaximumLength(200).WithMessage("Title must not exceed 200 characters.")
                .NotEmpty().WithMessage("Title is required.");

            RuleFor(v => v.Content)
                .NotEmpty().WithMessage("Content is required.");
        }
    }
}
