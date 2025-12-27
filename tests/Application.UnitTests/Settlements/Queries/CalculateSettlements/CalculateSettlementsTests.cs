using Application.Common.Mappings;
using Application.Settlements.Queries.CalculateSettlements;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Settlements.Queries.CalculateSettlements;

/// <summary>
/// 精算算出機能の単体テスト（TDD: Red段階）。
/// </summary>
public class CalculateSettlementsTests
{
    private readonly Mock<ITripRepository> _tripRepositoryMock;
    private readonly Mock<IPaymentRepository> _paymentRepositoryMock;

    public CalculateSettlementsTests()
    {
        _tripRepositoryMock = new Mock<ITripRepository>();
        _paymentRepositoryMock = new Mock<IPaymentRepository>();
    }

    [Fact]
    public async Task Handle_Should_CalculateCorrectly_For_SimpleCase()
    {
        // Arrange
        var tripId = Guid.NewGuid();
        var userA = Guid.NewGuid();
        var userB = Guid.NewGuid();
        var userC = Guid.NewGuid();

        var trip = Trip.Create(tripId, "test trip", DateTime.Today, DateTime.Today.AddDays(1), userA);
        trip.AddMember(TripMember.Create(Guid.NewGuid(), tripId, userA, "Admin"));
        trip.AddMember(TripMember.Create(Guid.NewGuid(), tripId, userB, "Member"));
        trip.AddMember(TripMember.Create(Guid.NewGuid(), tripId, userC, "Member"));

        _tripRepositoryMock.Setup(x => x.GetByIdAsync(tripId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(trip);

        var payment = Payment.Create(Guid.NewGuid(), tripId, userA, Guid.Empty, Money.Create(3000), "Dinner", DateTime.Now);
        payment.AddParticipant(userA);
        payment.AddParticipant(userB);
        payment.AddParticipant(userC);

        _paymentRepositoryMock.Setup(x => x.GetByTripIdAsync(tripId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Payment> { payment });

        var query = new CalculateSettlementsQuery(tripId);
        var handler = new CalculateSettlementsQueryHandler(_tripRepositoryMock.Object, _paymentRepositoryMock.Object);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(s => s.FromUserId == userB && s.ToUserId == userA && s.Amount == 1000);
        result.Should().Contain(s => s.FromUserId == userC && s.ToUserId == userA && s.Amount == 1000);
    }
}
