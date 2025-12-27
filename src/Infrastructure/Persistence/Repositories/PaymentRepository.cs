using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

/// <summary>
/// 支払い記録を管理するリポジトリの具象クラス。
/// </summary>
public class PaymentRepository : IPaymentRepository
{
    private readonly ApplicationDbContext _context;

    public PaymentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Payment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Payments.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Payment>> GetByTripIdAsync(Guid tripId, CancellationToken cancellationToken = default)
    {
        return await _context.Payments
            .Where(p => p.TripId == tripId)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Payment payment, CancellationToken cancellationToken = default)
    {
        await _context.Payments.AddAsync(payment, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Payment payment, CancellationToken cancellationToken = default)
    {
        _context.Payments.Update(payment);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Payment payment, CancellationToken cancellationToken = default)
    {
        _context.Payments.Remove(payment);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
