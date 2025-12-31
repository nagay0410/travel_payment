using Application.Auth.Commands.Login;
using Application.Auth.Common;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Auth.Commands.Login;

/// <summary>
/// ログイン機能の単体テスト。
/// </summary>
public class LoginCommandTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly Mock<IJwtTokenGenerator> _jwtTokenGeneratorMock;
    private readonly LoginCommandHandler _handler;

    public LoginCommandTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _jwtTokenGeneratorMock = new Mock<IJwtTokenGenerator>();
        _handler = new LoginCommandHandler(
            _userRepositoryMock.Object,
            _passwordHasherMock.Object,
            _jwtTokenGeneratorMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnAuthenticationResult_When_CredentialsAreValid()
    {
        // Arrange
        var email = Email.Create("test@example.com");
        var user = User.Create("testuser", email, "hashedPassword");
        var command = new LoginCommand("test@example.com", "password123");
        var expectedToken = "generated.jwt.token";

        _userRepositoryMock
            .Setup(x => x.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _passwordHasherMock
            .Setup(x => x.VerifyPassword(command.Password, user.PasswordHash))
            .Returns(true);

        _jwtTokenGeneratorMock
            .Setup(x => x.GenerateToken(user))
            .Returns(expectedToken);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.UserId.Should().Be(user.Id);
        result.UserName.Should().Be("testuser");
        result.Email.Should().Be("test@example.com");
        result.Token.Should().Be(expectedToken);

        _userRepositoryMock.Verify(x => x.UpdateAsync(user, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_ThrowUnauthorizedAccessException_When_UserNotFound()
    {
        // Arrange
        var command = new LoginCommand("notfound@example.com", "password123");

        _userRepositoryMock
            .Setup(x => x.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("メールアドレスまたはパスワードが正しくありません。");
    }

    [Fact]
    public async Task Handle_Should_ThrowUnauthorizedAccessException_When_PasswordIsInvalid()
    {
        // Arrange
        var email = Email.Create("test@example.com");
        var user = User.Create("testuser", email, "hashedPassword");
        var command = new LoginCommand("test@example.com", "wrongpassword");

        _userRepositoryMock
            .Setup(x => x.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _passwordHasherMock
            .Setup(x => x.VerifyPassword(command.Password, user.PasswordHash))
            .Returns(false);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("メールアドレスまたはパスワードが正しくありません。");
    }

    [Fact]
    public async Task Handle_Should_ThrowUnauthorizedAccessException_When_AccountIsInactive()
    {
        // Arrange
        var email = Email.Create("test@example.com");
        var user = User.Create("testuser", email, "hashedPassword");
        user.Deactivate(); // アカウントを無効化
        var command = new LoginCommand("test@example.com", "password123");

        _userRepositoryMock
            .Setup(x => x.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("このアカウントは無効です。");
    }
}
