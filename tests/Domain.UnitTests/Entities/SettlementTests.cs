using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.UnitTests.Entities;

public class SettlementTests
{
    [Fact]
    public void Create_WithValidParameters_ShouldInitializeCorrectly()
    {
        // Arrange
        var settlementId = Guid.NewGuid();
        var tripId = Guid.NewGuid();
        var fromUserId = Guid.NewGuid();
        var toUserId = Guid.NewGuid();
        var amount = 2500.00m;

        // Act
        var settlement = Settlement.Create(tripId, fromUserId, toUserId, Money.Create(amount));

        // Assert
        Assert.Equal(Guid.Empty, settlement.Id);
        Assert.Equal(tripId, settlement.TripId);
        Assert.Equal(fromUserId, settlement.FromUserId);
        Assert.Equal(toUserId, settlement.ToUserId);
        Assert.Equal(amount, settlement.Amount.Amount);
        Assert.False(settlement.IsCompleted); // 初期状態は未完了
        Assert.Null(settlement.CompletedAt);
    }

    [Fact]
    public void Complete_ShouldUpdateStatus()
    {
        // Arrange
        var settlement = Settlement.Create(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Money.Create(100));
        var method = "PayPay";

        // Act
        settlement.Complete(method);

        // Assert
        Assert.True(settlement.IsCompleted);
        Assert.Equal(method, settlement.SettlementMethod);
        Assert.NotNull(settlement.CompletedAt);
    }
}
