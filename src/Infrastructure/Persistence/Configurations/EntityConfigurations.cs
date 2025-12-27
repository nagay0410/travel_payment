using Domain.Entities;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

/// <summary>
/// Userエンティティの基本構成。
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasColumnName("user_id")
            .ValueGeneratedNever();

        builder.Property(u => u.UserName)
            .HasColumnName("user_name")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(u => u.Email)
            .HasColumnName("email")
            .HasMaxLength(255)
            .IsRequired()
            .HasConversion<EmailConverter>();

        builder.Property(u => u.PasswordHash)
            .HasColumnName("password_hash")
            .IsRequired();

        builder.Property(u => u.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        builder.Property(u => u.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(u => u.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();
    }
}

/// <summary>
/// Tripエンティティの構成。
/// </summary>
public class TripConfiguration : IEntityTypeConfiguration<Trip>
{
    public void Configure(EntityTypeBuilder<Trip> builder)
    {
        builder.ToTable("trips");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasColumnName("trip_id")
            .ValueGeneratedNever();

        builder.Property(t => t.TripName)
            .HasColumnName("trip_name")
            .HasMaxLength(100)
            .IsRequired();

        builder.OwnsOne(t => t.Budget, b =>
        {
            b.Property(p => p.Amount).HasColumnName("budget_amount");
            b.Property(p => p.Currency).HasColumnName("budget_currency").HasMaxLength(3);
        });

        // IReadOnlyCollection のマッピング（EF Core 8+ では簡略化されているが、必要に応じて設定）
        builder.HasMany(t => t.Members)
            .WithOne()
            .HasForeignKey(m => m.TripId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

/// <summary>
/// Paymentエンティティの構成。
/// </summary>
public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("payments");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("payment_id")
            .ValueGeneratedNever();

        builder.OwnsOne(p => p.Amount, a =>
        {
            a.Property(m => m.Amount).HasColumnName("amount");
            a.Property(m => m.Currency).HasColumnName("currency").HasMaxLength(3);
        });
    }
}
