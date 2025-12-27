using Domain.Common;
using Domain.ValueObjects;

namespace Domain.Entities;

/// <summary>
/// 旅行中の個々の支払いを管理するエンティティ。
/// </summary>
public class Payment : Entity
{
    /// <summary>
    /// 関連する旅行のID。
    /// </summary>
    public Guid TripId { get; private set; }

    /// <summary>
    /// 支払ったユーザーのID。
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// 支払いカテゴリのID。
    /// </summary>
    public Guid CategoryId { get; private set; }

    /// <summary>
    /// 支払い金額（金額と通貨のセット）。
    /// </summary>
    public Money Amount { get; private set; } = null!;

    /// <summary>
    /// 支払いの詳細説明。
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// 領収書画像のパスまたはURL（任意）。
    /// </summary>
    public string? ReceiptImage { get; private set; }

    /// <summary>
    /// 支払いが実際に行われた日付。
    /// </summary>
    public DateTime PaymentDate { get; private set; }

    /// <summary>
    /// レコード作成日時。
    /// </summary>
    public DateTimeOffset CreatedAt { get; private set; }

    /// <summary>
    /// レコード最終更新日時。
    /// </summary>
    public DateTimeOffset UpdatedAt { get; private set; }

    private readonly List<PaymentParticipant> _participants = new();

    /// <summary>
    /// 特定の支払いにおける精算対象者の一覧（読み取り専用）。
    /// </summary>
    public IReadOnlyCollection<PaymentParticipant> Participants => _participants.AsReadOnly();

    private Payment(Guid id, Guid tripId, Guid userId, Guid categoryId, Money amount, string? description, DateTime paymentDate, string? receiptImage) : base(id)
    {
        TripId = tripId;
        UserId = userId;
        CategoryId = categoryId;
        Amount = amount;
        Description = description;
        PaymentDate = paymentDate;
        ReceiptImage = receiptImage;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = CreatedAt;
    }

    // EF Core用
    private Payment() { }

    /// <summary>
    /// 新しい支払い記録を作成します。
    /// </summary>
    public static Payment Create(Guid id, Guid tripId, Guid userId, Guid categoryId, Money amount, string? description, DateTime paymentDate, string? receiptImage = null)
    {
        return new Payment(id, tripId, userId, categoryId, amount, description, paymentDate, receiptImage);
    }

    /// <summary>
    /// 支払い記録に精算対象者を追加します。
    /// </summary>
    /// <param name="userId">対象ユーザーのID</param>
    public void AddParticipant(Guid userId)
    {
        if (_participants.Any(p => p.UserId == userId))
            return;

        _participants.Add(PaymentParticipant.Create(Guid.NewGuid(), Id, userId));
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
