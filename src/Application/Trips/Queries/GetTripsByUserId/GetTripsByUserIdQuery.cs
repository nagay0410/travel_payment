using Application.Trips.Queries;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.Trips.Queries.GetTripsByUserId;

/// <summary>
/// 特定のユーザーが参加している旅行一覧を取得するクエリ。
/// </summary>
/// <param name="UserId">ユーザーID</param>
public record GetTripsByUserIdQuery(Guid UserId) : IRequest<IReadOnlyList<TripDto>>;

/// <summary>
/// <see cref="GetTripsByUserIdQuery"/> を処理するハンドラー。
/// </summary>
public class GetTripsByUserIdQueryHandler : IRequestHandler<GetTripsByUserIdQuery, IReadOnlyList<TripDto>>
{
    private readonly ITripRepository _tripRepository;
    private readonly IMapper _mapper;

    public GetTripsByUserIdQueryHandler(ITripRepository tripRepository, IMapper mapper)
    {
        _tripRepository = tripRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<TripDto>> Handle(GetTripsByUserIdQuery request, CancellationToken cancellationToken)
    {
        var trips = await _tripRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        
        return _mapper.Map<IReadOnlyList<TripDto>>(trips);
    }
}
