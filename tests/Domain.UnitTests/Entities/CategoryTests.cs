using Domain.Entities;

namespace Domain.UnitTests.Entities;

public class CategoryTests
{
    [Fact]
    public void Create_WithValidParameters_ShouldInitializeCorrectly()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var categoryName = "Food";
        var description = "Meals and drinks";
        var icon = "restaurant";

        // Act
        var category = Category.Create(categoryId, categoryName, description, icon);

        // Assert
        Assert.Equal(categoryId, category.Id);
        Assert.Equal(categoryName, category.CategoryName);
        Assert.Equal(description, category.Description);
        Assert.Equal(icon, category.Icon);
    }

    [Fact]
    public void Create_WithEmptyName_ShouldThrowArgumentException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() => 
            Category.Create(Guid.NewGuid(), "", "Desc", "icon"));
    }
}
