using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.UnitTests.Entities;

public class UserTests
{
    [Fact]
    public void Create_WithValidParameters_ShouldInitializeCorrectly()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var userName = "testuser";
        var email = "test@example.com";
        var passwordHash = "hashed_password";

        // Act
        var user = User.Create(userName, Email.Create(email), passwordHash);

        // Assert
        Assert.Equal(Guid.Empty, user.Id);
        Assert.Equal(userName, user.UserName);
        Assert.Equal(email, user.Email.Value);
        Assert.Equal(passwordHash, user.PasswordHash);
        Assert.True(user.IsActive);
        Assert.NotEqual(default, user.CreatedAt);
        Assert.NotEqual(default, user.UpdatedAt);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("  ")]
    public void Create_WithInvalidUserName_ShouldThrowArgumentException(string? invalidUserName)
    {
        // Arrange
        var userId = Guid.NewGuid();
        var email = "test@example.com";
        var passwordHash = "hashed_password";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => User.Create(invalidUserName!, Email.Create(email), passwordHash));
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("invalid-email")]
    public void Create_WithInvalidEmail_ShouldThrowArgumentException(string? invalidEmail)
    {
        // Arrange
        var userId = Guid.NewGuid();
        var userName = "testuser";
        var passwordHash = "hashed_password";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => User.Create(userName, Email.Create(invalidEmail!), passwordHash));
    }
}
