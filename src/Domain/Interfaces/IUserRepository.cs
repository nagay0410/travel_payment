using Domain.Entities;

namespace Domain.Interfaces;

/// <summary>
/// ユーザー情報を管理するリポジトリのインターフェース。
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// IDに基づいてユーザーを取得します。
    /// </summary>
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// メールアドレスに基づいてユーザーを取得します。
    /// </summary>
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// 新しいユーザーを追加します。
    /// </summary>
    Task AddAsync(User user, CancellationToken cancellationToken = default);

    /// <summary>
    /// ユーザー情報を更新します。
    /// </summary>
    Task UpdateAsync(User user, CancellationToken cancellationToken = default);
}
