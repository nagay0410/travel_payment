using Application.Settlements.Queries;
using Domain.Interfaces;
using MediatR;

namespace Application.Settlements.Queries.CalculateSettlements;

/// <summary>
/// 旅行の精算結果（誰が誰にいくら払うか）を算出するクエリ。
/// </summary>
/// <param name="TripId">旅行ID</param>
public record CalculateSettlementsQuery(Guid TripId) : IRequest<IReadOnlyList<SettlementDto>>;

/// <summary>
/// <see cref="CalculateSettlementsQuery"/> を処理するハンドラー。
/// </summary>
public class CalculateSettlementsQueryHandler : IRequestHandler<CalculateSettlementsQuery, IReadOnlyList<SettlementDto>>
{
    private readonly ITripRepository _tripRepository;
    private readonly IPaymentRepository _paymentRepository;

    public CalculateSettlementsQueryHandler(ITripRepository tripRepository, IPaymentRepository paymentRepository)
    {
        _tripRepository = tripRepository;
        _paymentRepository = paymentRepository;
    }

    public async Task<IReadOnlyList<SettlementDto>> Handle(CalculateSettlementsQuery request, CancellationToken cancellationToken)
    {
        var trip = await _tripRepository.GetByIdAsync(request.TripId, cancellationToken);
        if (trip == null) throw new Exception("Trip not found");

        var payments = await _paymentRepository.GetByTripIdAsync(request.TripId, cancellationToken);

        // 各ユーザーのネットバランスを算出
        var balances = new Dictionary<Guid, decimal>();
        foreach (var member in trip.Members)
        {
            balances[member.UserId] = 0;
        }

        foreach (var payment in payments)
        {
            var amount = payment.Amount.Amount;
            var participantIds = payment.Participants.Select(p => p.UserId).ToList();
            if (participantIds.Count == 0) continue;

            var perPerson = amount / participantIds.Count;

            // 支払い者のプラス
            if (balances.ContainsKey(payment.UserId))
                balances[payment.UserId] += amount;

            // 参加者のマイナス
            foreach (var pid in participantIds)
            {
                if (balances.ContainsKey(pid))
                    balances[pid] -= perPerson;
            }
        }

        // 精算トランザクションの作成（簡易アルゴリズム）
        var debtors = balances.Where(x => x.Value < -0.01m).OrderBy(x => x.Value).ToList();
        var creditors = balances.Where(x => x.Value > 0.01m).OrderByDescending(x => x.Value).ToList();

        var results = new List<SettlementDto>();
        var dIdx = 0;
        var cIdx = 0;

        while (dIdx < debtors.Count && cIdx < creditors.Count)
        {
            var debtorId = debtors[dIdx].Key;
            var creditorId = creditors[cIdx].Key;
            
            var debt = Math.Abs(debtors[dIdx].Value);
            var credit = creditors[cIdx].Value;

            var settleAmount = Math.Min(debt, credit);
            
            results.Add(new SettlementDto
            {
                FromUserId = debtorId,
                ToUserId = creditorId,
                Amount = Math.Round(settleAmount, 0), // 円単位なら四捨五入
                Currency = "JPY"
            });

            // 残高更新（簡易的なため、実際にはリストの状態を更新しながらループするのが正確）
            // ここでは1対1の単純マッチングをシミュレート
            if (debt > credit)
            {
                debtors[dIdx] = new KeyValuePair<Guid, decimal>(debtorId, -(debt - credit));
                cIdx++;
            }
            else if (debt < credit)
            {
                creditors[cIdx] = new KeyValuePair<Guid, decimal>(creditorId, credit - debt);
                dIdx++;
            }
            else
            {
                dIdx++;
                cIdx++;
            }
        }

        return results;
    }
}
