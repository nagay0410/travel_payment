using Application.Common.Interfaces;
using Application.Users.Commands.CreateUser;
using Domain.ValueObjects;
using Domain.Entities;
using Domain.Common;
using Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Users.Commands.CreateUser;

/// <summary>
/// ユーザー登録機能の単体テスト。
/// </summary>
public class CreateUserCommandTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;

    public CreateUserCommandTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
    }

    [Fact]
    public async Task Handle_Should_CreateUser_When_RequestIsValid()
    {
        // Arrange
        var command = new CreateUserCommand("testuser", "test@example.com", "password123");
        var hashedPassword = "hashed_password_123";

        _passwordHasherMock
            .Setup(x => x.HashPassword(command.Password))
            .Returns(hashedPassword);

        var handler = new CreateUserCommandHandler(_userRepositoryMock.Object, _passwordHasherMock.Object);

        _userRepositoryMock.Setup(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .Callback<User, CancellationToken>((u, c) => typeof(Entity).GetProperty("Id")!.SetValue(u, Guid.NewGuid()));

        _userRepositoryMock.Setup(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .Callback<User, CancellationToken>((u, c) => typeof(Entity).GetProperty("Id")!.SetValue(u, Guid.NewGuid()));

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeEmpty();
        _passwordHasherMock.Verify(x => x.HashPassword(command.Password), Times.Once);
        _userRepositoryMock.Verify(x => x.AddAsync(
            It.Is<User>(u => u.PasswordHash == hashedPassword),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}

