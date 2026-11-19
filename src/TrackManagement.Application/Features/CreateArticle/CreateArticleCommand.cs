using FluentValidation;

namespace TrackManagement.Application.Features.CreateArticle;

public record CreateArticleCommand(
    int JournalId,
    string Title,
    string Scope

    );


public class CreateArticleCommandValidator : AbstractValidator<CreateArticleCommand>
{
    public CreateArticleCommandValidator()
    {

    }
}
