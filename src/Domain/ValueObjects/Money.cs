using Domain.Common;

namespace Domain.ValueObjects;

/// <summary>
/// 金額と通貨をセットにした金銭を表す値オブジェクト。
/// </summary>
public class Money : ValueObject
{
    /// <summary>
    /// 数値としての金額。
    /// </summary>
    public decimal Amount { get; }

    /// <summary>
    /// ISO 4217 通貨コード。
    /// </summary>
    public string Currency { get; }

    private Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    /// <summary>
    /// 新しい金銭オブジェクトを作成します。
    /// 金額が負の場合は例外をスローします。
    /// </summary>
    /// <param name="amount">金額（0以上）</param>
    /// <param name="currency">通貨コード（デフォルト: JPY）</param>
    /// <returns>Moneyインスタンス</returns>
    /// <exception cref="ArgumentException">金額が不正な場合</exception>
    public static Money Create(decimal amount, string currency = "JPY")
    {
        if (amount < 0)
            throw new ArgumentException("金額は0以上である必要があります。", nameof(amount));

        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("通貨は必須です。", nameof(currency));

        return new Money(amount, currency);
    }

    /// <summary>
    /// 等価性の比較に使用するコンポーネントを返します。
    /// </summary>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }

    /// <summary>
    /// 文字列表現を返します。
    /// </summary>
    public override string ToString() => $"{Amount} {Currency}";
}
