using FluentValidation;

namespace Application.Trips.Commands.AddMember;

/// <summary>
/// <see cref="AddMemberCommand"/> のバリデーター。
/// </summary>
public class AddMemberCommandValidator : AbstractValidator<AddMemberCommand>
{
    public AddMemberCommandValidator()
    {
        RuleFor(v => v.TripId)
            .NotEmpty().WithMessage("旅行IDは必須です。");

        RuleFor(v => v.Email)
            .NotEmpty().WithMessage("メールアドレスは必須です。")
            .EmailAddress().WithMessage("有効なメールアドレスを入力してください。");

        RuleFor(v => v.Role)
            .NotEmpty().WithMessage("ロールは必須です。")
            .Must(role => new[] { "Admin", "Member", "Viewer" }.Contains(role))
            .WithMessage("ロールは 'Admin', 'Member', 'Viewer' のいずれかである必要があります。");
    }
}
