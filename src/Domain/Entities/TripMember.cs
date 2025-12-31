using Domain.Common;

namespace Domain.Entities;

/// <summary>
/// 旅行への参加メンバーを管理するエンティティ。
/// </summary>
public class TripMember : Entity
{
    private static readonly string[] AllowedRoles = { "Admin", "Member", "Viewer" };

    /// <summary>
    /// 関連する旅行のID。
    /// </summary>
    public Guid TripId { get; private set; }

    /// <summary>
    /// ユーザーのID。
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// メンバーのロール（Admin, Member, Viewer）。
    /// </summary>
    public string Role { get; private set; } = string.Empty;

    /// <summary>
    /// 参加日時。
    /// </summary>
    public DateTimeOffset JoinedAt { get; private set; }

    /// <summary>
    /// メンバーシップが有効かどうか。
    /// </summary>
    public bool IsActive { get; private set; }

    private TripMember(Guid tripId, Guid userId, string role) : base()
    {
        TripId = tripId;
        UserId = userId;
        Role = role;
        JoinedAt = DateTimeOffset.UtcNow;
        IsActive = true;
    }

    // EF Core用
    private TripMember() { }

    /// <summary>
    /// 新しい旅行メンバーを作成します。
    /// </summary>
    /// <param name="id">メンバーシップID</param>
    /// <param name="tripId">旅行ID</param>
    /// <param name="userId">ユーザーID</param>
    /// <param name="role">ロール</param>
    /// <returns>TripMemberインスタンス</returns>
    /// <exception cref="ArgumentException">不正なロールの場合</exception>
    public static TripMember Create(Guid tripId, Guid userId, string role)
    {
        if (!AllowedRoles.Contains(role))
            throw new ArgumentException($"不正なロールです。許可されているロール: {string.Join(", ", AllowedRoles)}", nameof(role));

        return new TripMember(tripId, userId, role);
    }

    /// <summary>
    /// メンバーを非活性化（脱退など）します。
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
    }
}
