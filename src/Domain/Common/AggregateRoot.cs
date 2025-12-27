namespace Domain.Common;

/// <summary>
/// 集約ルートの基底クラス。
/// 整合性を維持する境界のルートとなるエンティティです。
/// </summary>
public abstract class AggregateRoot : Entity
{
    /// <summary>
    /// 指定したIDで集約ルートを初期化します。
    /// </summary>
    /// <param name="id">識別子</param>
    protected AggregateRoot(Guid id) : base(id) { }

    /// <summary>
    /// EF Coreなどのツール用。
    /// </summary>
    protected AggregateRoot() : base() { }
}
