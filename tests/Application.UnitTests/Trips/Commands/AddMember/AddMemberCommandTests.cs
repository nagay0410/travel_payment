using Application.Trips.Commands.AddMember;
using Domain.Entities;
using Domain.Common;
using Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Trips.Commands.AddMember;

/// <summary>
/// 旅行メンバー追加機能の単体テスト（TDD: Red段階）。
/// </summary>
public class AddMemberCommandTests
{
    private readonly Mock<ITripRepository> _tripRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;

    public AddMemberCommandTests()
    {
        _tripRepositoryMock = new Mock<ITripRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
    }

    [Fact]
    public async Task Handle_Should_AddMember_When_RequestIsValid()
    {
        // Arrange
        var tripId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var trip = Trip.Create("test trip", DateTime.Today, DateTime.Today.AddDays(1), Guid.NewGuid());
        typeof(Entity).GetProperty("Id")!.SetValue(trip, tripId);

        var user = User.Create("new member", Domain.ValueObjects.Email.Create("member@example.com"), "hash");
        typeof(Entity).GetProperty("Id")!.SetValue(user, userId);

        _tripRepositoryMock.Setup(x => x.GetByIdAsync(tripId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(trip);
        _userRepositoryMock.Setup(x => x.GetByEmailAsync("member@example.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var command = new AddMemberCommand(tripId, "member@example.com", "Member");
        var handler = new AddMemberCommandHandler(_tripRepositoryMock.Object, _userRepositoryMock.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        trip.Members.Should().Contain(m => m.UserId == userId);
        _tripRepositoryMock.Verify(x => x.UpdateAsync(trip, It.IsAny<CancellationToken>()), Times.Once);
    }
}
