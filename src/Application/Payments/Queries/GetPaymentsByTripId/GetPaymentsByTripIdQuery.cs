using Application.Payments.Queries;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.Payments.Queries.GetPaymentsByTripId;

/// <summary>
/// 旅行IDを指定して支払い一覧を取得するクエリ。
/// </summary>
/// <param name="TripId">旅行ID</param>
public record GetPaymentsByTripIdQuery(Guid TripId) : IRequest<IReadOnlyList<PaymentDto>>;

/// <summary>
/// <see cref="GetPaymentsByTripIdQuery"/> を処理するハンドラー。
/// </summary>
public class GetPaymentsByTripIdQueryHandler : IRequestHandler<GetPaymentsByTripIdQuery, IReadOnlyList<PaymentDto>>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IMapper _mapper;

    public GetPaymentsByTripIdQueryHandler(IPaymentRepository paymentRepository, IMapper mapper)
    {
        _paymentRepository = paymentRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<PaymentDto>> Handle(GetPaymentsByTripIdQuery request, CancellationToken cancellationToken)
    {
        var payments = await _paymentRepository.GetByTripIdAsync(request.TripId, cancellationToken);
        
        return _mapper.Map<IReadOnlyList<PaymentDto>>(payments);
    }
}
