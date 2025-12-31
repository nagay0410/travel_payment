using Application.Common.Mappings;
using Application.Payments.Commands.CreatePayment;
using Domain.Entities;
using Domain.Common;
using Domain.Interfaces;
using Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Payments.Commands.CreatePayment;

/// <summary>
/// 支払い登録機能の単体テスト（TDD: Red段階）。
/// </summary>
public class CreatePaymentTests
{
    private readonly Mock<IPaymentRepository> _paymentRepositoryMock;
    private readonly Mock<ITripRepository> _tripRepositoryMock;

    public CreatePaymentTests()
    {
        _paymentRepositoryMock = new Mock<IPaymentRepository>();
        _tripRepositoryMock = new Mock<ITripRepository>();
    }

    [Fact]
    public async Task Handle_Should_CreatePayment_When_RequestIsValid()
    {
        // Arrange
        var tripId = Guid.NewGuid();
        var payerId = Guid.NewGuid();
        var trip = Trip.Create("test trip", DateTime.Today, DateTime.Today.AddDays(1), Guid.NewGuid());
        typeof(Entity).GetProperty("Id")!.SetValue(trip, tripId);

        // メンバーとして追加しておく必要がある
        trip.AddMember(TripMember.Create(tripId, payerId, "Member"));

        _tripRepositoryMock.Setup(x => x.GetByIdAsync(tripId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(trip);

        var command = new CreatePaymentCommand(tripId, payerId, "Lunch", 5000, "JPY", DateTime.Now, new List<Guid> { payerId });
        var handler = new CreatePaymentCommandHandler(_paymentRepositoryMock.Object, _tripRepositoryMock.Object);

        _paymentRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Payment>(), It.IsAny<CancellationToken>()))
            .Callback<Payment, CancellationToken>((p, c) => typeof(Entity).GetProperty("Id")!.SetValue(p, Guid.NewGuid()));

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeEmpty();
        _paymentRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Payment>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
