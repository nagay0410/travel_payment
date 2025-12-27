using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

/// <summary>
/// 精算結果を管理するリポジトリの具象クラス。
/// </summary>
public class SettlementRepository : ISettlementRepository
{
    private readonly ApplicationDbContext _context;

    public SettlementRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Settlement?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Settlements.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Settlement>> GetByTripIdAsync(Guid tripId, CancellationToken cancellationToken = default)
    {
        return await _context.Settlements
            .Where(s => s.TripId == tripId)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Settlement settlement, CancellationToken cancellationToken = default)
    {
        await _context.Settlements.AddAsync(settlement, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<Settlement> settlements, CancellationToken cancellationToken = default)
    {
        await _context.Settlements.AddRangeAsync(settlements, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Settlement settlement, CancellationToken cancellationToken = default)
    {
        _context.Settlements.Update(settlement);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
