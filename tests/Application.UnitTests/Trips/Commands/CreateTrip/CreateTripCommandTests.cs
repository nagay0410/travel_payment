using Application.Trips.Commands.CreateTrip;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Trips.Commands.CreateTrip;

/// <summary>
/// 旅行作成機能の単体テスト（TDD: Red段階）。
/// </summary>
public class CreateTripCommandTests
{
    private readonly Mock<ITripRepository> _tripRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;

    public CreateTripCommandTests()
    {
        _tripRepositoryMock = new Mock<ITripRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
    }

    [Fact]
    public async Task Handle_Should_CreateTrip_When_RequestIsValid()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = User.Create(userId, "testuser", Email.Create("test@example.com"), "hash");
        _userRepositoryMock.Setup(x => x.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var startDate = DateTime.Today;
        var endDate = DateTime.Today.AddDays(3);
        var command = new CreateTripCommand("test trip", userId, startDate, endDate, 100000, "JPY");
        var handler = new CreateTripCommandHandler(_tripRepositoryMock.Object, _userRepositoryMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeEmpty();
        _tripRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Trip>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_ThrowException_When_UserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _userRepositoryMock.Setup(x => x.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        var startDate = DateTime.Today;
        var endDate = DateTime.Today.AddDays(3);
        var command = new CreateTripCommand("test trip", userId, startDate, endDate, 100000, "JPY");
        var handler = new CreateTripCommandHandler(_tripRepositoryMock.Object, _userRepositoryMock.Object);

        // Act & Assert
        await handler.Invoking(x => x.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<Exception>();
    }
}
