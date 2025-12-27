using FluentValidation;

namespace Application.Trips.Commands.CreateTrip;

/// <summary>
/// <see cref="CreateTripCommand"/> のバリデーター。
/// </summary>
public class CreateTripCommandValidator : AbstractValidator<CreateTripCommand>
{
    public CreateTripCommandValidator()
    {
        RuleFor(v => v.Title)
            .MaximumLength(100).WithMessage("タイトルは100文字以内で入力してください。")
            .NotEmpty().WithMessage("タイトルは必須です。");

        RuleFor(v => v.OwnerId)
            .NotEmpty().WithMessage("作成者IDは必須です。");

        RuleFor(v => v.StartDate)
            .NotEmpty().WithMessage("開始日は必須です。");

        RuleFor(v => v.EndDate)
            .NotEmpty().WithMessage("終了日は必須です。")
            .GreaterThanOrEqualTo(v => v.StartDate).WithMessage("終了日は開始日以降である必要があります。");

        RuleFor(v => v.BudgetAmount)
            .GreaterThanOrEqualTo(0).WithMessage("予算額は0以上である必要があります。");

        RuleFor(v => v.Currency)
            .NotEmpty().WithMessage("通貨は必須です。")
            .Length(3).WithMessage("通貨コードは3文字（例: JPY）で入力してください。");
    }
}
