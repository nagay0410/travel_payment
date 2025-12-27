using Domain.Entities;

namespace Domain.Interfaces;

/// <summary>
/// 支払い記録を管理するリポジトリのインターフェース。
/// </summary>
public interface IPaymentRepository
{
    /// <summary>
    /// IDに基づいて支払い情報を取得します。
    /// </summary>
    Task<Payment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// 旅行に関連するすべての支払い記録を取得します。
    /// </summary>
    Task<IReadOnlyList<Payment>> GetByTripIdAsync(Guid tripId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 新しい支払い記録を追加します。
    /// </summary>
    Task AddAsync(Payment payment, CancellationToken cancellationToken = default);

    /// <summary>
    /// 支払い記録を更新します。
    /// </summary>
    Task UpdateAsync(Payment payment, CancellationToken cancellationToken = default);

    /// <summary>
    /// 支払い記録を削除します。
    /// </summary>
    Task DeleteAsync(Payment payment, CancellationToken cancellationToken = default);
}
