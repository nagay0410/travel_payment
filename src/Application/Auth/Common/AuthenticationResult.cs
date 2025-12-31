namespace Application.Auth.Common;

/// <summary>
/// 認証結果を表すレコード。
/// </summary>
/// <param name="UserId">認証されたユーザーのID</param>
/// <param name="UserName">認証されたユーザーの名前</param>
/// <param name="Email">認証されたユーザーのメールアドレス</param>
/// <param name="Token">生成されたJWTトークン</param>
public record AuthenticationResult(
    Guid UserId,
    string UserName,
    string Email,
    string Token);
