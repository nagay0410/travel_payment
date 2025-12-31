using Domain.Entities;
using Domain.ValueObjects;
using MediatR;
using Domain.Interfaces;

namespace Application.Payments.Commands.CreatePayment;

/// <summary>
/// 支払い記録を登録するコマンド。
/// </summary>
/// <param name="TripId">旅行ID</param>
/// <param name="PayerId">支払ったユーザーのID</param>
/// <param name="Title">支払いの内容（例: 昼食代）</param>
/// <param name="Amount">金額</param>
/// <param name="Currency">通貨（例: JPY）</param>
/// <param name="PaymentDate">支払い日</param>
/// <param name="ParticipantIds">精算対象となるメンバーのユーザーID一覧</param>
public record CreatePaymentCommand(
    Guid TripId,
    Guid PayerId,
    string Title,
    decimal Amount,
    string Currency,
    DateTime PaymentDate,
    IEnumerable<Guid> ParticipantIds,
    string? ReceiptImage = null) : IRequest<Guid>;

/// <summary>
/// <see cref="CreatePaymentCommand"/> を処理するハンドラー。
/// </summary>
public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, Guid>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly ITripRepository _tripRepository;

    public CreatePaymentCommandHandler(IPaymentRepository paymentRepository, ITripRepository tripRepository)
    {
        _paymentRepository = paymentRepository;
        _tripRepository = tripRepository;
    }

    public async Task<Guid> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        var trip = await _tripRepository.GetByIdAsync(request.TripId, cancellationToken);
        if (trip == null)
        {
            throw new Exception($"Trip with ID {request.TripId} not found.");
        }

        // 支払い者がメンバーに含まれているか確認
        if (trip.Members.All(m => m.UserId != request.PayerId))
        {
            throw new Exception($"Payer with ID {request.PayerId} is not a member of the trip.");
        }

        var amount = Money.Create(request.Amount, request.Currency);
        // カテゴリ機能は未実装のため、一旦 Guid.Empty またはデフォルトの仕組みが必要
        // Domain で Category も Entity なので本来は存在するはず
        var categoryId = Guid.Empty; // TODO: カテゴリ管理機能実装後に修正

        var payment = Payment.Create(
            request.TripId,
            request.PayerId,
            categoryId,
            amount,
            request.Title,
            request.PaymentDate,
            request.ReceiptImage);

        // 精算対象者の追加
        foreach (var participantId in request.ParticipantIds)
        {
            payment.AddParticipant(participantId);
        }

        await _paymentRepository.AddAsync(payment, cancellationToken);

        return payment.Id;
    }
}
