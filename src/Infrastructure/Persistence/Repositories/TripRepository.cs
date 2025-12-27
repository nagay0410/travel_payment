using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

/// <summary>
/// 旅行プロジェクトを管理するリポジトリの具象クラス。
/// </summary>
public class TripRepository : ITripRepository
{
    private readonly ApplicationDbContext _context;

    public TripRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Trip?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Trips
            .Include(t => t.Members)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Trip>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Trips
            .Where(t => t.Members.Any(m => m.UserId == userId))
            .Include(t => t.Members)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Trip trip, CancellationToken cancellationToken = default)
    {
        await _context.Trips.AddAsync(trip, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Trip trip, CancellationToken cancellationToken = default)
    {
        _context.Trips.Update(trip);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Trip trip, CancellationToken cancellationToken = default)
    {
        _context.Trips.Remove(trip);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
