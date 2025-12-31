namespace Infrastructure.Auth;

/// <summary>
/// JWT設定を保持するクラス。
/// </summary>
public class JwtSettings
{
    /// <summary>
    /// 設定セクション名。
    /// </summary>
    public const string SectionName = "JwtSettings";

    /// <summary>
    /// シークレットキー。
    /// </summary>
    public string Secret { get; set; } = string.Empty;

    /// <summary>
    /// トークン発行者。
    /// </summary>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// トークン対象者。
    /// </summary>
    public string Audience { get; set; } = string.Empty;

    /// <summary>
    /// アクセストークンの有効期限（分）。
    /// </summary>
    public int ExpiryMinutes { get; set; } = 60;
}
