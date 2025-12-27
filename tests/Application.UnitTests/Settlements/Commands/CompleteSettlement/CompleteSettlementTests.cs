using Application.Settlements.Commands.CompleteSettlement;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Settlements.Commands.CompleteSettlement;

/// <summary>
/// 精算完了機能の単体テスト（TDD: Red段階）。
/// </summary>
public class CompleteSettlementTests
{
    private readonly Mock<ISettlementRepository> _settlementRepositoryMock;

    public CompleteSettlementTests()
    {
        _settlementRepositoryMock = new Mock<ISettlementRepository>();
    }

    [Fact]
    public async Task Handle_Should_CreateCompletedSettlement_When_RequestIsValid()
    {
        // Arrange
        var tripId = Guid.NewGuid();
        var fromUserId = Guid.NewGuid();
        var toUserId = Guid.NewGuid();
        var command = new CompleteSettlementCommand(tripId, fromUserId, toUserId, 1000, "JPY", "PayPay");
        var handler = new CompleteSettlementCommandHandler(_settlementRepositoryMock.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        _settlementRepositoryMock.Verify(x => x.AddAsync(It.Is<Settlement>(s => 
            s.TripId == tripId && 
            s.FromUserId == fromUserId && 
            s.ToUserId == toUserId && 
            s.Amount.Amount == 1000 &&
            s.IsCompleted), It.IsAny<CancellationToken>()), Times.Once);
    }
}
