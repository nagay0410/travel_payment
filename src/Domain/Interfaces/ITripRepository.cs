using Domain.Entities;

namespace Domain.Interfaces;

/// <summary>
/// 旅行プロジェクトを管理するリポジトリのインターフェース。
/// </summary>
public interface ITripRepository
{
    /// <summary>
    /// IDに基づいて旅行情報を取得します（メンバー情報を含む）。
    /// </summary>
    Task<Trip?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// 特定のユーザーが参加している旅行一覧を取得します。
    /// </summary>
    Task<IReadOnlyList<Trip>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 新しい旅行を追加します。
    /// </summary>
    Task AddAsync(Trip trip, CancellationToken cancellationToken = default);

    /// <summary>
    /// 旅行情報を更新します。
    /// </summary>
    Task UpdateAsync(Trip trip, CancellationToken cancellationToken = default);

    /// <summary>
    /// 旅行を削除します。
    /// </summary>
    Task DeleteAsync(Trip trip, CancellationToken cancellationToken = default);
}
