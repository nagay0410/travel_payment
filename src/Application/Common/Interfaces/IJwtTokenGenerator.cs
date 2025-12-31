using Domain.Entities;

namespace Application.Common.Interfaces;

/// <summary>
/// JWTトークンを生成するためのインターフェース。
/// </summary>
public interface IJwtTokenGenerator
{
    /// <summary>
    /// 指定されたユーザー情報からJWTトークンを生成します。
    /// </summary>
    /// <param name="user">トークン生成対象のユーザー</param>
    /// <returns>生成されたJWTトークン文字列</returns>
    string GenerateToken(User user);
}
