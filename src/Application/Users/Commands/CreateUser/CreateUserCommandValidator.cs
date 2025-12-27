using FluentValidation;

namespace Application.Users.Commands.CreateUser;

/// <summary>
/// <see cref="CreateUserCommand"/> のバリデーター。
/// </summary>
public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(v => v.UserName)
            .MaximumLength(50).WithMessage("ユーザー名は50文字以内で入力してください。")
            .NotEmpty().WithMessage("ユーザー名は必須です。");

        RuleFor(v => v.Email)
            .NotEmpty().WithMessage("メールアドレスは必須です。")
            .EmailAddress().WithMessage("有効なメールアドレスを入力してください。");

        RuleFor(v => v.Password)
            .MinimumLength(6).WithMessage("パスワードは6文字以上で入力してください。")
            .NotEmpty().WithMessage("パスワードは必須です。");
    }
}
