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
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("uuid_generate_v4()");

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
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("uuid_generate_v4()");

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
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("uuid_generate_v4()");

        builder.OwnsOne(p => p.Amount, a =>
        {
            a.Property(m => m.Amount).HasColumnName("amount");
            a.Property(m => m.Currency).HasColumnName("currency").HasMaxLength(3);
        });

        builder.HasMany(p => p.Participants)
            .WithOne()
            .HasForeignKey(p => p.PaymentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class PaymentParticipantConfiguration : IEntityTypeConfiguration<PaymentParticipant>
{
    public void Configure(EntityTypeBuilder<PaymentParticipant> builder)
    {
        builder.ToTable("payment_participants");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("payment_participant_id")
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("uuid_generate_v4()");

        builder.Property(x => x.PaymentId)
            .HasColumnName("payment_id")
            .IsRequired();

        builder.Property(x => x.UserId)
            .HasColumnName("user_id")
            .IsRequired();
    }
}

public class TripMemberConfiguration : IEntityTypeConfiguration<TripMember>
{
    public void Configure(EntityTypeBuilder<TripMember> builder)
    {
        builder.ToTable("trip_members");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("trip_member_id")
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("uuid_generate_v4()");
        builder.Property(x => x.TripId).HasColumnName("trip_id").IsRequired();
        builder.Property(x => x.UserId).HasColumnName("user_id").IsRequired();
        builder.Property(x => x.Role).HasColumnName("role").IsRequired();
        builder.Property(x => x.JoinedAt).HasColumnName("joined_at").IsRequired();
        builder.Property(x => x.IsActive).HasColumnName("is_active").IsRequired();
    }
}

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("categories");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("category_id")
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("uuid_generate_v4()");
        builder.Property(x => x.CategoryName).HasColumnName("category_name").HasMaxLength(50).IsRequired();
        builder.Property(x => x.Description).HasColumnName("description").HasMaxLength(200);
        builder.Property(x => x.Icon).HasColumnName("icon").HasMaxLength(50);
    }
}

public class SettlementConfiguration : IEntityTypeConfiguration<Settlement>
{
    public void Configure(EntityTypeBuilder<Settlement> builder)
    {
        builder.ToTable("settlements");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("settlement_id")
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("uuid_generate_v4()");
        builder.Property(x => x.TripId).HasColumnName("trip_id").IsRequired();
        builder.Property(x => x.FromUserId).HasColumnName("from_user_id").IsRequired();
        builder.Property(x => x.ToUserId).HasColumnName("to_user_id").IsRequired();

        builder.OwnsOne(s => s.Amount, a =>
        {
            a.Property(m => m.Amount).HasColumnName("amount");
            a.Property(m => m.Currency).HasColumnName("currency").HasMaxLength(3);
        });

        builder.Property(x => x.SettlementMethod).HasColumnName("settlement_method").HasMaxLength(50);
        builder.Property(x => x.IsCompleted).HasColumnName("is_completed").IsRequired();
        builder.Property(x => x.CompletedAt).HasColumnName("completed_at");
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
    }
}
