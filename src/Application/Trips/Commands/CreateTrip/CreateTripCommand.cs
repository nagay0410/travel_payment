using Domain.Entities;
using Domain.ValueObjects;
using MediatR;
using Domain.Interfaces;

namespace Application.Trips.Commands.CreateTrip;

/// <summary>
/// 新しい旅行プロジェクトを作成するコマンド。
/// </summary>
/// <param name="Title">旅行のタイトル</param>
/// <param name="OwnerId">作成者（オーナー）のユーザーID</param>
/// <param name="StartDate">開始日</param>
/// <param name="EndDate">終了日</param>
/// <param name="BudgetAmount">予算額</param>
/// <param name="Currency">通貨コード（例: JPY）</param>
public record CreateTripCommand(
    string Title,
    Guid OwnerId,
    DateTime StartDate,
    DateTime EndDate,
    decimal BudgetAmount,
    string Currency) : IRequest<Guid>;

/// <summary>
/// <see cref="CreateTripCommand"/> を処理するハンドラー。
/// </summary>
public class CreateTripCommandHandler : IRequestHandler<CreateTripCommand, Guid>
{
    private readonly ITripRepository _tripRepository;
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// 新しいインスタンスを生成します。
    /// </summary>
    /// <param name="tripRepository">旅行リポジトリ</param>
    /// <param name="userRepository">ユーザーリポジトリ</param>
    public CreateTripCommandHandler(ITripRepository tripRepository, IUserRepository userRepository)
    {
        _tripRepository = tripRepository;
        _userRepository = userRepository;
    }

    /// <summary>
    /// 旅行プロジェクトの作成を実行します。
    /// </summary>
    /// <param name="request">作成リクエスト</param>
    /// <param name="cancellationToken">キャンセル・トークン</param>
    /// <returns>作成された旅行プロジェクトのID</returns>
    /// <exception cref="Exception">ユーザーが見つからない場合にスローされます</exception>
    public async Task<Guid> Handle(CreateTripCommand request, CancellationToken cancellationToken)
    {
        var owner = await _userRepository.GetByIdAsync(request.OwnerId, cancellationToken);
        if (owner == null)
        {
            throw new Exception($"User with ID {request.OwnerId} not found.");
        }

        var budget = Money.Create(request.BudgetAmount, request.Currency);
        var trip = Trip.Create(
            Guid.NewGuid(),
            request.Title,
            request.StartDate,
            request.EndDate,
            owner.Id,
            null,
            budget);

        // 作成者をメンバーとして追加（Adminロールを使用）
        var ownerMember = TripMember.Create(Guid.NewGuid(), trip.Id, owner.Id, "Admin");
        trip.AddMember(ownerMember);

        await _tripRepository.AddAsync(trip, cancellationToken);

        return trip.Id;
    }
}
