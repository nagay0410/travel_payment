using FluentValidation;

namespace Application.Payments.Commands.CreatePayment;

/// <summary>
/// <see cref="CreatePaymentCommand"/> のバリデーター。
/// </summary>
public class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
{
    public CreatePaymentCommandValidator()
    {
        RuleFor(v => v.TripId)
            .NotEmpty().WithMessage("旅行IDは必須です。");

        RuleFor(v => v.PayerId)
            .NotEmpty().WithMessage("支払い者IDは必須です。");

        RuleFor(v => v.Title)
            .NotEmpty().WithMessage("タイトルは必須です。")
            .MaximumLength(100).WithMessage("タイトルは100文字以内で入力してください。");

        RuleFor(v => v.Amount)
            .GreaterThan(0).WithMessage("金額は0より大きい必要があります。");

        RuleFor(v => v.Currency)
            .NotEmpty().WithMessage("通貨は必須です。")
            .Length(3).WithMessage("通貨コードは3文字で入力してください。");

        RuleFor(v => v.ParticipantIds)
            .NotEmpty().WithMessage("精算対象者は1人以上指定してください。");

        RuleFor(v => v.ReceiptImage)
            .MaximumLength(3000000).WithMessage("領収書画像が大きすぎます（最大 2MB 程度）。");
    }
}
