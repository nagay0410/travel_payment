using Domain.Common;

namespace Domain.Entities;

/// <summary>
/// 特定の支払いにおける精算対象者（参加者）を管理するエンティティ。
/// </summary>
public class PaymentParticipant : Entity
{
    /// <summary>
    /// 関連する支払いのID。
    /// </summary>
    public Guid PaymentId { get; private set; }

    /// <summary>
    /// 精算対象となるユーザーのID。
    /// </summary>
    public Guid UserId { get; private set; }

    private PaymentParticipant(Guid paymentId, Guid userId) : base()
    {
        PaymentId = paymentId;
        UserId = userId;
    }

    // EF Core用
    private PaymentParticipant() { }

    /// <summary>
    /// 新しい支払い参加者を作成します。
    /// </summary>
    /// <param name="id">参加記録ID</param>
    /// <param name="paymentId">支払いID</param>
    /// <param name="userId">ユーザーID</param>
    /// <returns>PaymentParticipantインスタンス</returns>
    public static PaymentParticipant Create(Guid paymentId, Guid userId)
    {
        return new PaymentParticipant(paymentId, userId);
    }
}
