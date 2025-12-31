using Application.Common.Interfaces;
using Domain.Entities;
using Domain.ValueObjects;
using MediatR;
using Domain.Interfaces;

namespace Application.Users.Commands.CreateUser;

/// <summary>
/// ユーザー登録を行うためのコマンド。
/// </summary>
/// <param name="UserName">希望するユーザー名</param>
/// <param name="Email">メールアドレス</param>
/// <param name="Password">パスワード（平文）</param>
public record CreateUserCommand(string UserName, string Email, string Password) : IRequest<Guid>;

/// <summary>
/// <see cref="CreateUserCommand"/> を処理するハンドラー。
/// </summary>
public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    /// <summary>
    /// 新しいインスタンスを生成します。
    /// </summary>
    /// <param name="userRepository">ユーザーリポジトリ</param>
    /// <param name="passwordHasher">パスワードハッシュ化サービス</param>
    public CreateUserCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    /// <summary>
    /// ユーザー登録処理を実行します。
    /// </summary>
    /// <param name="request">登録リクエスト</param>
    /// <param name="cancellationToken">キャンセル・トークン</param>
    /// <returns>作成されたユーザーのID</returns>
    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        // 値オブジェクトの生成時にバリデーションが行われる（Domain層の責務）
        var email = Email.Create(request.Email);

        // パスワードをハッシュ化
        var passwordHash = _passwordHasher.HashPassword(request.Password);

        var user = User.Create(request.UserName, email, passwordHash);

        await _userRepository.AddAsync(user, cancellationToken);

        return user.Id;
    }
}

