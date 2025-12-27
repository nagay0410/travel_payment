using System.Reflection;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

/// <summary>
/// アプリケーションのデータベースコンテキスト。
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Trip> Trips => Set<Trip>();
    public DbSet<TripMember> TripMembers => Set<TripMember>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<PaymentParticipant> PaymentParticipants => Set<PaymentParticipant>();
    public DbSet<Settlement> Settlements => Set<Settlement>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // PostgreSQL 固有の拡張設定（UUID生成など）が必要な場合はここに記述
        modelBuilder.HasPostgresExtension("uuid-ossp");

        // スネークケースへの自動変換（PostgreSQLの慣習に合わせる場合）
        // ただし Configuration で明示的に ToTable/HasColumnName を行う場合はそちらが優先される

        base.OnModelCreating(modelBuilder);
    }
}
