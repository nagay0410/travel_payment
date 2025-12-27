using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Persistence.Converters;

/// <summary>
/// Email値オブジェクトを文字列に変換するコンバーター。
/// </summary>
public class EmailConverter : ValueConverter<Email, string>
{
    public EmailConverter() : base(
        email => email.Value,
        value => Email.Create(value))
    {
    }
}

/// <summary>
/// Money値オブジェクト（金額部分）を数値に変換するコンバーター。
/// ※通貨コードとセットで扱う場合は Owned Entity または個別のコンバーターを検討。
/// 今回は単純化のため Amount のみをマッピング対象とする例。
/// </summary>
public class MoneyAmountConverter : ValueConverter<Money, decimal>
{
    public MoneyAmountConverter() : base(
        money => money.Amount,
        value => Money.Create(value, "JPY")) // 通貨の復元は設計に合わせて要調整
    {
    }
}
