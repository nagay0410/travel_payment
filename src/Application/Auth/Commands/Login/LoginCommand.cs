using Application.Auth.Common;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Domain.Interfaces;

namespace Application.Auth.Commands.Login;

/// <summary>
/// ログイン認証を行うコマンド。
/// </summary>
/// <param name="Email">メールアドレス</param>
/// <param name="Password">パスワード（平文）</param>
public record LoginCommand(string Email, string Password) : IRequest<AuthenticationResult>;

/// <summary>
/// <see cref="LoginCommand"/> を処理するハンドラー。
/// </summary>
public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthenticationResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    /// <summary>
    /// 新しいインスタンスを生成します。
    /// </summary>
    /// <param name="userRepository">ユーザーリポジトリ</param>
    /// <param name="passwordHasher">パスワードハッシュ化サービス</param>
    /// <param name="jwtTokenGenerator">JWTトークン生成サービス</param>
    public LoginCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    /// <summary>
    /// ログイン認証処理を実行します。
    /// </summary>
    /// <param name="request">ログインリクエスト</param>
    /// <param name="cancellationToken">キャンセル・トークン</param>
    /// <returns>認証結果</returns>
    /// <exception cref="UnauthorizedAccessException">認証に失敗した場合</exception>
    public async Task<AuthenticationResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // メールアドレスでユーザーを検索
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

        // ユーザーが存在しない場合
        if (user is null)
        {
            throw new UnauthorizedAccessException("メールアドレスまたはパスワードが正しくありません。");
        }

        // アカウントが無効な場合
        if (!user.IsActive)
        {
            throw new UnauthorizedAccessException("このアカウントは無効です。");
        }

        // パスワードを検証
        if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("メールアドレスまたはパスワードが正しくありません。");
        }

        // 最終ログイン日時を更新
        user.UpdateLastLogin();
        await _userRepository.UpdateAsync(user, cancellationToken);

        // JWTトークンを生成
        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(
            user.Id,
            user.UserName,
            user.Email.Value,
            token);
    }
}

