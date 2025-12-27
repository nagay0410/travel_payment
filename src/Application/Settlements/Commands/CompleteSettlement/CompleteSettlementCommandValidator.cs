using FluentValidation;

namespace Application.Settlements.Commands.CompleteSettlement;

/// <summary>
/// <see cref="CompleteSettlementCommand"/> のバリデーター。
/// </summary>
public class CompleteSettlementCommandValidator : AbstractValidator<CompleteSettlementCommand>
{
    public CompleteSettlementCommandValidator()
    {
        RuleFor(v => v.TripId)
            .NotEmpty().WithMessage("旅行IDは必須です。");

        RuleFor(v => v.FromUserId)
            .NotEmpty().WithMessage("支払者IDは必須です。");

        RuleFor(v => v.ToUserId)
            .NotEmpty().WithMessage("受取者IDは必須です。")
            .NotEqual(v => v.FromUserId).WithMessage("支払者と受取者は別人である必要があります。");

        RuleFor(v => v.Amount)
            .GreaterThan(0).WithMessage("金額は0より大きい必要があります。");

        RuleFor(v => v.Currency)
            .NotEmpty().WithMessage("通貨は必須です。")
            .Length(3).WithMessage("通貨コードは3文字で入力してください。");

        RuleFor(v => v.Method)
            .NotEmpty().WithMessage("支払い方法は必須です。");
    }
}
