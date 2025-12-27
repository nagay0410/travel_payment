using Domain.Common;
using Domain.ValueObjects;

namespace Domain.Entities;

/// <summary>
/// 旅行プロジェクトを管理する集約ルート。
/// 参加メンバーやステータスを保持します。
/// </summary>
public class Trip : AggregateRoot
{
    private readonly List<TripMember> _members = new();

    /// <summary>
    /// 旅行名。
    /// </summary>
    public string TripName { get; private set; } = string.Empty;

    /// <summary>
    /// 旅行の詳細説明。
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// 旅行期間の開始日。
    /// </summary>
    public DateTime StartDate { get; private set; }

    /// <summary>
    /// 旅行期間の終了日。
    /// </summary>
    public DateTime EndDate { get; private set; }

    /// <summary>
    /// 旅行の予算。
    /// </summary>
    public Money? Budget { get; private set; }

    /// <summary>
    /// 旅行の現在のステータス（Planning, Active, Completed, Cancelled）。
    /// </summary>
    public string Status { get; private set; } = string.Empty;

    /// <summary>
    /// 作成者のユーザーID。
    /// </summary>
    public Guid CreatedBy { get; private set; }

    /// <summary>
    /// 作成日時。
    /// </summary>
    public DateTimeOffset CreatedAt { get; private set; }

    /// <summary>
    /// 最終更新日時。
    /// </summary>
    public DateTimeOffset UpdatedAt { get; private set; }

    /// <summary>
    /// 旅行に参加しているメンバーの一覧（読み取り専用）。
    /// </summary>
    public IReadOnlyCollection<TripMember> Members => _members.AsReadOnly();

    private Trip(Guid id, string tripName, DateTime startDate, DateTime endDate, Guid createdBy, string? description, Money? budget) : base(id)
    {
        TripName = tripName;
        StartDate = startDate;
        EndDate = endDate;
        CreatedBy = createdBy;
        Description = description;
        Budget = budget;
        Status = "Planning";
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = CreatedAt;
    }

    // EF Core用
    private Trip() { }

    /// <summary>
    /// 新しい旅行プロジェクトを作成します。
    /// </summary>
    /// <param name="id">旅行ID</param>
    /// <param name="tripName">旅行名</param>
    /// <param name="startDate">開始日</param>
    /// <param name="endDate">終了日</param>
    /// <param name="createdBy">作成者ID</param>
    /// <param name="description">説明（任意）</param>
    /// <param name="budget">予算（任意）</param>
    /// <returns>Tripインスタンス</returns>
    /// <exception cref="ArgumentException">日付の前後関係が不正な場合など</exception>
    public static Trip Create(Guid id, string tripName, DateTime startDate, DateTime endDate, Guid createdBy, string? description = null, Money? budget = null)
    {
        if (string.IsNullOrWhiteSpace(tripName))
            throw new ArgumentException("旅行名は必須です。", nameof(tripName));

        if (endDate < startDate)
            throw new ArgumentException("終了日は開始日以降である必要があります。", nameof(endDate));

        return new Trip(id, tripName, startDate, endDate, createdBy, description, budget);
    }

    /// <summary>
    /// 旅行にメンバーを追加します。
    /// </summary>
    /// <param name="member">追加するメンバーエンティティ</param>
    /// <exception cref="InvalidOperationException">既に登録済みの場合など</exception>
    public void AddMember(TripMember member)
    {
        if (member.TripId != Id)
            throw new InvalidOperationException("この旅行のメンバーではありません。");

        if (_members.Any(m => m.UserId == member.UserId))
            throw new InvalidOperationException("既にメンバーとして登録されています。");

        _members.Add(member);
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// 旅行のステータスを更新します。
    /// </summary>
    /// <param name="status">新ステータス</param>
    public void UpdateStatus(string status)
    {
        if (string.IsNullOrWhiteSpace(status))
            throw new ArgumentException("ステータスは必須です。", nameof(status));

        Status = status;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
