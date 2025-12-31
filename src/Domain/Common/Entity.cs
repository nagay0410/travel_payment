namespace Domain.Common;

/// <summary>
/// エンティティの基底クラス。
/// ID（一意の識別子）に基づいて同一性を判断します。
/// </summary>
public abstract class Entity
{
    /// <summary>
    /// エンティティの一意識別子。
    /// </summary>
    public Guid Id { get; protected set; }

    /// <summary>
    /// 指定したIDでエンティティを初期化します。
    /// </summary>
    /// <param name="id">識別子</param>
    protected Entity(Guid id)
    {
        Id = id;
    }

    /// <summary>
    /// 新規作成用（IDはDBで採番されるため指定しない）。
    /// </summary>
    protected Entity() { }

    /// <summary>
    /// 指定したオブジェクトが現在のエンティティと同一かどうかを判断します。
    /// 同一性は型とIDによって決まります。
    /// </summary>
    public override bool Equals(object? obj)
    {
        if (obj is not Entity other)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (GetType() != other.GetType())
            return false;

        if (Id == Guid.Empty || other.Id == Guid.Empty)
            return false;

        return Id == other.Id;
    }

    /// <summary>
    /// 等価演算子のオーバーロード。
    /// </summary>
    public static bool operator ==(Entity? a, Entity? b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    /// <summary>
    /// 非等価演算子のオーバーロード。
    /// </summary>
    public static bool operator !=(Entity? a, Entity? b)
    {
        return !(a == b);
    }

    /// <summary>
    /// このエンティティのハッシュ値を計算します。
    /// </summary>
    public override int GetHashCode()
    {
        return (GetType().ToString() + Id).GetHashCode();
    }
}
