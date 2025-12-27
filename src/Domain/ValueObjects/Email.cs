using System.Text.RegularExpressions;
using Domain.Common;

namespace Domain.ValueObjects;

/// <summary>
/// メールアドレスを表す値オブジェクト。
/// バリデーション済みであることを保証します。
/// </summary>
public class Email : ValueObject
{
    /// <summary>
    /// メールアドレスの文字列値。
    /// </summary>
    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    /// <summary>
    /// 新しいメールアドレスを作成します。
    /// 不正な形式の場合は例外をスローします。
    /// </summary>
    /// <param name="value">メールアドレス文字列</param>
    /// <returns>Emailインスタンス</returns>
    /// <exception cref="ArgumentException">不正なメールアドレスの場合</exception>
    public static Email Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("メールアドレスは必須です。", nameof(value));

        var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        if (!regex.IsMatch(value))
            throw new ArgumentException("有効なメールアドレスを入力してください。", nameof(value));

        return new Email(value);
    }

    /// <summary>
    /// 等価性の比較に使用するコンポーネントを返します。
    /// ケースインセンシティブに比較します。
    /// </summary>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value.ToLowerInvariant();
    }

    /// <summary>
    /// 文字列表現を返します。
    /// </summary>
    public override string ToString() => Value;
}
