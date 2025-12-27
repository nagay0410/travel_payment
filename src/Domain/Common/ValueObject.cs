namespace Domain.Common;

/// <summary>
/// 値オブジェクトの基底クラス。
/// 保持する値の内容に基づいて等価性を判断します（不変性を前提とします）。
/// </summary>
public abstract class ValueObject
{
    /// <summary>
    /// 等価演算子の共通ロジック。
    /// </summary>
    protected static bool EqualOperator(ValueObject? left, ValueObject? right)
    {
        if (left is null ^ right is null)
        {
            return false;
        }

        return left?.Equals(right!) != false;
    }

    /// <summary>
    /// 非等価演算子の共通ロジック。
    /// </summary>
    protected static bool NotEqualOperator(ValueObject? left, ValueObject? right)
    {
        return !EqualOperator(left, right);
    }

    /// <summary>
    /// 比較対象となるプロパティの列挙を実装します。
    /// </summary>
    /// <returns>比較対象のオブジェクト群</returns>
    protected abstract IEnumerable<object?> GetEqualityComponents();

    /// <summary>
    /// 指定したオブジェクトが現在のオブジェクトと等しいかどうかを判断します。
    /// </summary>
    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
        {
            return false;
        }

        var other = (ValueObject)obj;

        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    /// <summary>
    /// このオブジェクトのハッシュ値を計算します。
    /// </summary>
    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(x => x?.GetHashCode() ?? 0)
            .Aggregate((x, y) => x ^ y);
    }

    /// <summary>
    /// 等価演算子のオーバーロード。
    /// </summary>
    public static bool operator ==(ValueObject? one, ValueObject? two)
    {
        return EqualOperator(one, two);
    }

    /// <summary>
    /// 非等価演算子のオーバーロード。
    /// </summary>
    public static bool operator !=(ValueObject? one, ValueObject? two)
    {
        return NotEqualOperator(one, two);
    }
}
