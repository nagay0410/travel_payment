using Application.Common.Mappings;
using Application.Trips.Queries;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.Trips.Queries.GetTripById;

/// <summary>
/// 指定されたIDの旅行詳細を取得するクエリ。
/// </summary>
/// <param name="Id">旅行ID</param>
public record GetTripByIdQuery(Guid Id) : IRequest<TripDto?>;

/// <summary>
/// <see cref="GetTripByIdQuery"/> を処理するハンドラー。
/// </summary>
public class GetTripByIdQueryHandler : IRequestHandler<GetTripByIdQuery, TripDto?>
{
    private readonly ITripRepository _tripRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// 新しいインスタンスを生成します。
    /// </summary>
    /// <param name="tripRepository">旅行リポジトリ</param>
    /// <param name="mapper">マッパー</param>
    public GetTripByIdQueryHandler(ITripRepository tripRepository, IMapper mapper)
    {
        _tripRepository = tripRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// 旅行情報の取得を実行します。
    /// </summary>
    /// <param name="request">クエリリクエスト</param>
    /// <param name="cancellationToken">キャンセル・トークン</param>
    /// <returns>旅行情報（存在しない場合はnull）</returns>
    public async Task<TripDto?> Handle(GetTripByIdQuery request, CancellationToken cancellationToken)
    {
        var trip = await _tripRepository.GetByIdAsync(request.Id, cancellationToken);
        
        return trip == null ? null : _mapper.Map<TripDto>(trip);
    }
}
