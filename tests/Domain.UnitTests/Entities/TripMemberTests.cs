using Domain.Entities;

namespace Domain.UnitTests.Entities;

public class TripMemberTests
{
    [Fact]
    public void Create_WithValidParameters_ShouldInitializeCorrectly()
    {
        // Arrange
        var tripMemberId = Guid.NewGuid();
        var tripId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var role = "Admin";

        // Act
        var member = TripMember.Create(tripMemberId, tripId, userId, role);

        // Assert
        Assert.Equal(tripMemberId, member.Id);
        Assert.Equal(tripId, member.TripId);
        Assert.Equal(userId, member.UserId);
        Assert.Equal(role, member.Role);
        Assert.True(member.IsActive);
    }

    [Theory]
    [InlineData("")]
    [InlineData("Invalid")]
    public void Create_WithInvalidRole_ShouldThrowArgumentException(string role)
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() => 
            TripMember.Create(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), role));
    }
}
