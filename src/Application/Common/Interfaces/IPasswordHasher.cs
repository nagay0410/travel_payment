namespace Application.Common.Interfaces;

/// <summary>
/// パスワードのハッシュ化と検証を行うためのインターフェース。
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    /// パスワードをハッシュ化します。
    /// </summary>
    /// <param name="password">ハッシュ化する平文パスワード</param>
    /// <returns>ハッシュ化されたパスワード</returns>
    string HashPassword(string password);

    /// <summary>
    /// パスワードを検証します。
    /// </summary>
    /// <param name="password">検証する平文パスワード</param>
    /// <param name="hashedPassword">保存されているハッシュ化パスワード</param>
    /// <returns>パスワードが一致する場合はtrue</returns>
    bool VerifyPassword(string password, string hashedPassword);
}
