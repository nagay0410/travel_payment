using Application.Common.Mappings;
using Application.Users.Commands.CreateUser;
using Domain.ValueObjects;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Users.Commands.CreateUser;

/// <summary>
/// ユーザー登録機能の単体テスト（TDD: Red段階）。
/// まだ実装（CreateUserCommand / Handler）が存在しないためビルドエラーになります。
/// </summary>
public class CreateUserCommandTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;

    public CreateUserCommandTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
    }

    [Fact]
    public async Task Handle_Should_CreateUser_When_RequestIsValid()
    {
        // Arrange
        var command = new CreateUserCommand("testuser", "test@example.com", "password123");
        var handler = new CreateUserCommandHandler(_userRepositoryMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeEmpty();
        _userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
