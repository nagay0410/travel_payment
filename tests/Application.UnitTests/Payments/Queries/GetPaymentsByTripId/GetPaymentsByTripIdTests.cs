using Application.Common.Mappings;
using Application.Payments.Queries.GetPaymentsByTripId;
using Application.Payments.Queries;
using Domain.Entities;
using Domain.Common;
using Domain.Interfaces;
using Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Payments.Queries.GetPaymentsByTripId;

/// <summary>
/// 支払い一覧取得機能の単体テスト（TDD: Red段階）。
/// </summary>
public class GetPaymentsByTripIdTests
{
    private readonly Mock<IPaymentRepository> _paymentRepositoryMock;
    private readonly AutoMapper.IMapper _mapper;

    public GetPaymentsByTripIdTests()
    {
        _paymentRepositoryMock = new Mock<IPaymentRepository>();

        var config = new AutoMapper.MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task Handle_Should_ReturnPayments_When_PaymentsExist()
    {
        // Arrange
        var tripId = Guid.NewGuid();
        var p = Payment.Create(tripId, Guid.NewGuid(), Guid.Empty, Money.Create(1000), "Dinner", DateTime.Now);
        typeof(Entity).GetProperty("Id")!.SetValue(p, Guid.NewGuid());
        var payments = new List<Payment> { p };
        _paymentRepositoryMock.Setup(x => x.GetByTripIdAsync(tripId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(payments);

        var query = new GetPaymentsByTripIdQuery(tripId);
        var handler = new GetPaymentsByTripIdQueryHandler(_paymentRepositoryMock.Object, _mapper);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(1);
        result[0].Title.Should().Be("Dinner");
    }
}
