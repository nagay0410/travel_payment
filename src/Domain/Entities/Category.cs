using Domain.Common;

namespace Domain.Entities;

/// <summary>
/// 支払いのカテゴリ（食費、交通費など）を定義するエンティティ。
/// </summary>
public class Category : Entity
{
    /// <summary>
    /// カテゴリ名。
    /// </summary>
    public string CategoryName { get; private set; } = string.Empty;

    /// <summary>
    /// カテゴリの説明。
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// 表示用アイコンの識別子。
    /// </summary>
    public string? Icon { get; private set; }

    private Category(string categoryName, string? description, string? icon) : base()
    {
        CategoryName = categoryName;
        Description = description;
        Icon = icon;
    }

    // EF Core用
    private Category() { }

    /// <summary>
    /// 新しいカテゴリを作成します。
    /// </summary>
    /// <param name="id">カテゴリID</param>
    /// <param name="categoryName">カテゴリ名</param>
    /// <param name="description">説明（任意）</param>
    /// <param name="icon">アイコン（任意）</param>
    /// <returns>Categoryインスタンス</returns>
    public static Category Create(string categoryName, string? description = null, string? icon = null)
    {
        if (string.IsNullOrWhiteSpace(categoryName))
            throw new ArgumentException("カテゴリ名は必須です。", nameof(categoryName));

        return new Category(categoryName, description, icon);
    }
}
