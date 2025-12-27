using Domain.Entities;

namespace Domain.Interfaces;

/// <summary>
/// 精算結果を管理するリポジトリのインターフェース。
/// </summary>
public interface ISettlementRepository
{
    /// <summary>
    /// IDに基づいて精算情報を取得します。
    /// </summary>
    Task<Settlement?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// 旅行に関連するすべての精算記録を取得します。
    /// </summary>
    Task<IReadOnlyList<Settlement>> GetByTripIdAsync(Guid tripId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 一括で精算データを追加します。
    /// </summary>
    Task AddRangeAsync(IEnumerable<Settlement> settlements, CancellationToken cancellationToken = default);

    /// <summary>
    /// 精算情報を更新します。
    /// </summary>
    Task UpdateAsync(Settlement settlement, CancellationToken cancellationToken = default);
}
