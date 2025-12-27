using Application.Common.Mappings;
using Application.Users.Queries.GetUserById;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Users.Queries.GetUserById;

/// <summary>
/// ユーザー取得機能の単体テスト（TDD: Red段階）。
/// </summary>
public class GetUserByIdQueryTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly AutoMapper.IMapper _mapper;

    public GetUserByIdQueryTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        
        var config = new AutoMapper.MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task Handle_Should_ReturnUser_When_UserExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = User.Create(userId, "testuser", Email.Create("test@example.com"), "hash");
        _userRepositoryMock.Setup(x => x.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var query = new GetUserByIdQuery(userId);
        var handler = new GetUserByIdQueryHandler(_userRepositoryMock.Object, _mapper);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(userId);
        result.UserName.Should().Be("testuser");
    }

    [Fact]
    public async Task Handle_Should_ReturnNull_When_UserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _userRepositoryMock.Setup(x => x.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        var query = new GetUserByIdQuery(userId);
        var handler = new GetUserByIdQueryHandler(_userRepositoryMock.Object, _mapper);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
}
