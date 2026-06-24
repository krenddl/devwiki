using FluentValidation;

namespace DevWiki.Application.Articles.Commands.CreateArticle
{
    public class CreateArticleCommandValidator : AbstractValidator<CreateArticleCommand>
    {
        public CreateArticleCommandValidator()
        {
            RuleFor(v => v.Title)
                .MaximumLength(200).WithMessage("Title must not exceed 200 characters.")
                .NotEmpty().WithMessage("Title is required.");

            RuleFor(v => v.Content)
                .NotEmpty().WithMessage("Content is required.");
        }
    }
}
