using Domain.Entities;
using Domain.ValueObjects;
using MediatR;
using Domain.Interfaces;

namespace Application.Settlements.Commands.CompleteSettlement;

/// <summary>
/// 特定の精算（支払い）を完了として記録するコマンド。
/// </summary>
/// <param name="TripId">旅行ID</param>
/// <param name="FromUserId">支払う側のユーザーID</param>
/// <param name="ToUserId">受け取る側のユーザーID</param>
/// <param name="Amount">金額</param>
/// <param name="Currency">通貨</param>
/// <param name="Method">支払い方法（PayPay等）</param>
public record CompleteSettlementCommand(
    Guid TripId, 
    Guid FromUserId, 
    Guid ToUserId, 
    decimal Amount, 
    string Currency, 
    string Method) : IRequest;

/// <summary>
/// <see cref="CompleteSettlementCommand"/> を処理するハンドラー。
/// </summary>
public class CompleteSettlementCommandHandler : IRequestHandler<CompleteSettlementCommand>
{
    private readonly ISettlementRepository _settlementRepository;

    public CompleteSettlementCommandHandler(ISettlementRepository settlementRepository)
    {
        _settlementRepository = settlementRepository;
    }

    public async Task Handle(CompleteSettlementCommand request, CancellationToken cancellationToken)
    {
        var amount = Money.Create(request.Amount, request.Currency);
        var settlement = Settlement.Create(
            Guid.NewGuid(), 
            request.TripId, 
            request.FromUserId, 
            request.ToUserId, 
            amount);

        settlement.Complete(request.Method);

        await _settlementRepository.AddAsync(settlement, cancellationToken);
    }
}
