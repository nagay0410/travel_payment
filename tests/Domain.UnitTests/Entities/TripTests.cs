using Domain.Entities;

namespace Domain.UnitTests.Entities;

public class TripTests
{
    [Fact]
    public void Create_WithValidParameters_ShouldInitializeCorrectly()
    {
        // Arrange
        var tripId = Guid.NewGuid();
        var tripName = "Japan Trip";
        var startDate = DateTime.Today;
        var endDate = DateTime.Today.AddDays(7);
        var createdBy = Guid.NewGuid();

        // Act
        var trip = Trip.Create(tripName, startDate, endDate, createdBy);

        // Assert
        Assert.Equal(Guid.Empty, trip.Id);
        Assert.Equal(tripName, trip.TripName);
        Assert.Equal(startDate, trip.StartDate);
        Assert.Equal(endDate, trip.EndDate);
        Assert.Equal(createdBy, trip.CreatedBy);
        Assert.Equal("Planning", trip.Status); // デフォルトステータス
    }

    [Fact]
    public void Create_WithEndDateBeforeStartDate_ShouldThrowArgumentException()
    {
        // Arrange
        var tripId = Guid.NewGuid();
        var tripName = "Invalid Trip";
        var startDate = DateTime.Today;
        var endDate = DateTime.Today.AddDays(-1);
        var createdBy = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => Trip.Create(tripName, startDate, endDate, createdBy));
    }

    [Fact]
    public void UpdateStatus_WithValidStatus_ShouldChangeStatus()
    {
        // Arrange
        var trip = Trip.Create("Test", DateTime.Today, DateTime.Today.AddDays(1), Guid.NewGuid());

        // Act
        trip.UpdateStatus("Active");

        // Assert
        Assert.Equal("Active", trip.Status);
    }
}
