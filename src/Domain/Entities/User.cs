using Domain.Common;
using Domain.ValueObjects;

namespace Domain.Entities;

/// <summary>
/// ユーザー情報を管理する集約ルート。
/// </summary>
public class User : AggregateRoot
{
    /// <summary>
    /// 表示名。
    /// </summary>
    public string UserName { get; private set; } = string.Empty;

    /// <summary>
    /// メールアドレス。
    /// </summary>
    public Email Email { get; private set; } = null!;

    /// <summary>
    /// パスワードのハッシュ値。
    /// </summary>
    public string PasswordHash { get; private set; } = string.Empty;

    /// <summary>
    /// 最終ログイン日時。
    /// </summary>
    public DateTimeOffset? LastLoginAt { get; private set; }

    /// <summary>
    /// アカウントが有効かどうか。
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// 登録日時。
    /// </summary>
    public DateTimeOffset CreatedAt { get; private set; }

    /// <summary>
    /// 最終更新日時。
    /// </summary>
    public DateTimeOffset UpdatedAt { get; private set; }

    private User(string userName, Email email, string passwordHash) : base()
    {
        UserName = userName;
        Email = email;
        PasswordHash = passwordHash;
        IsActive = true;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = CreatedAt;
    }

    // EF Core用
    private User() { }

    /// <summary>
    /// 新しいユーザーを作成します。
    /// </summary>
    /// <param name="userName">ユーザー名</param>
    /// <param name="email">メールアドレス（Email値オブジェクト）</param>
    /// <param name="passwordHash">パスワードハッシュ</param>
    /// <returns>Userインスタンス</returns>
    /// <exception cref="ArgumentException">入力値が不正な場合</exception>
    public static User Create(string userName, Email email, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(userName))
            throw new ArgumentException("ユーザー名は必須です。", nameof(userName));

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("パスワードハッシュは必須です。", nameof(passwordHash));

        return new User(userName, email, passwordHash);
    }

    /// <summary>
    /// 最終ログイン日時を現在時刻に更新します。
    /// </summary>
    public void UpdateLastLogin()
    {
        LastLoginAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// ユーザーを非活性化します。
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
