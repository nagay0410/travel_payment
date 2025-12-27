using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.UnitTests.Entities;

public class PaymentTests
{
    [Fact]
    public void Create_WithValidParameters_ShouldInitializeCorrectly()
    {
        // Arrange
        var paymentId = Guid.NewGuid();
        var tripId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();
        var amount = 1500.50m;
        var description = "Lunch at Tokyo";
        var paymentDate = DateTime.Now;

        // Act
        var payment = Payment.Create(paymentId, tripId, userId, categoryId, Money.Create(amount), description, paymentDate);

        // Assert
        Assert.Equal(paymentId, payment.Id);
        Assert.Equal(tripId, payment.TripId);
        Assert.Equal(userId, payment.UserId);
        Assert.Equal(categoryId, payment.CategoryId);
        Assert.Equal(amount, payment.Amount.Amount);
        Assert.Equal(description, payment.Description);
        Assert.Equal(paymentDate, payment.PaymentDate);
        Assert.Equal("JPY", payment.Amount.Currency); // デフォルト
    }

    [Fact]
    public void Create_WithZeroAmount_ShouldReturnCorrectMoney()
    {
        // 0はMoneyでは許可（精算用など）だが、Paymentとしてはバリデーションを追加してもよい。
        // ここではMoneyの仕様に合わせるか、Payment固有のチェックを設ける。
        // 現在のMoney.Createは0以上。
        var money = Money.Create(0);
        Assert.Equal(0, money.Amount);
    }

    [Fact]
    public void Create_WithNegativeAmount_ShouldThrowArgumentException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() => 
            Payment.Create(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Money.Create(-10), "Bad", DateTime.Now));
    }
}
