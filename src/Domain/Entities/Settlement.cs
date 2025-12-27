using Domain.Common;
using Domain.ValueObjects;

namespace Domain.Entities;

/// <summary>
/// 旅行終了後のユーザー間の貸し借り（精算）を管理するエンティティ。
/// </summary>
public class Settlement : Entity
{
    /// <summary>
    /// 関連する旅行のID。
    /// </summary>
    public Guid TripId { get; private set; }

    /// <summary>
    /// 誰から（支払う側）のユーザーID。
    /// </summary>
    public Guid FromUserId { get; private set; }

    /// <summary>
    /// 誰へ（受け取る側）のユーザーID。
    /// </summary>
    public Guid ToUserId { get; private set; }

    /// <summary>
    /// 精算金額。
    /// </summary>
    public Money Amount { get; private set; } = null!;

    /// <summary>
    /// 精算に使用した方法（PayPay, 銀行振込など）。
    /// </summary>
    public string? SettlementMethod { get; private set; }

    /// <summary>
    /// 精算が完了（支払い済み）かどうか。
    /// </summary>
    public bool IsCompleted { get; private set; }

    /// <summary>
    /// 精算完了が記録された日時。
    /// </summary>
    public DateTimeOffset? CompletedAt { get; private set; }

    /// <summary>
    /// レコード作成日時。
    /// </summary>
    public DateTimeOffset CreatedAt { get; private set; }

    private Settlement(Guid id, Guid tripId, Guid fromUserId, Guid toUserId, Money amount) : base(id)
    {
        TripId = tripId;
        FromUserId = fromUserId;
        ToUserId = toUserId;
        Amount = amount;
        IsCompleted = false;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    // EF Core用
    private Settlement() { }

    /// <summary>
    /// 新しい精算データをレコード作成します。
    /// </summary>
    /// <param name="id">精算ID</param>
    /// <param name="tripId">旅行ID</param>
    /// <param name="fromUserId">支払者ユーザーID</param>
    /// <param name="toUserId">受取者ユーザーID</param>
    /// <param name="amount">精算金額</param>
    /// <returns>Settlementインスタンス</returns>
    public static Settlement Create(Guid id, Guid tripId, Guid fromUserId, Guid toUserId, Money amount)
    {
        return new Settlement(id, tripId, fromUserId, toUserId, amount);
    }

    /// <summary>
    /// 精算を完了状態に更新します。
    /// </summary>
    /// <param name="settlementMethod">支払い方法</param>
    public void Complete(string settlementMethod)
    {
        IsCompleted = true;
        SettlementMethod = settlementMethod;
        CompletedAt = DateTimeOffset.UtcNow;
    }
}
