using FluentValidation;
using TaskManagement.Application.DTOs.Reactions;

namespace TaskManagement.Application.Validators
{
    public class CreateReactionDtoValidator : AbstractValidator<CreateReactionDto>
    {
        public CreateReactionDtoValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0);

            RuleFor(x => x.ReactionTypeId)
                .GreaterThan(0);

            RuleFor(x => x)
                .Must(x => (x.TaskId.HasValue && !x.CommentId.HasValue) ||
                           (!x.TaskId.HasValue && x.CommentId.HasValue))
                .WithMessage("Reaction must belong to either Task or Comment, not both");
        }
    }
}
