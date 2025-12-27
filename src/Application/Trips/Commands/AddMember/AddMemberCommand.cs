using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Trips.Commands.AddMember;

/// <summary>
/// 旅行に新しいメンバーを追加するコマンド。
/// </summary>
/// <param name="TripId">追加先の旅行ID</param>
/// <param name="Email">追加したいユーザーのメールアドレス</param>
/// <param name="Role">ロール（Admin, Member, Viewer）</param>
public record AddMemberCommand(Guid TripId, string Email, string Role) : IRequest;

/// <summary>
/// <see cref="AddMemberCommand"/> を処理するハンドラー。
/// </summary>
public class AddMemberCommandHandler : IRequestHandler<AddMemberCommand>
{
    private readonly ITripRepository _tripRepository;
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// 新しいインスタンスを生成します。
    /// </summary>
    /// <param name="tripRepository">旅行リポジトリ</param>
    /// <param name="userRepository">ユーザーリポジトリ</param>
    public AddMemberCommandHandler(ITripRepository tripRepository, IUserRepository userRepository)
    {
        _tripRepository = tripRepository;
        _userRepository = userRepository;
    }

    /// <summary>
    /// メンバー追加処理を実行します。
    /// </summary>
    /// <param name="request">追加リクエスト</param>
    /// <param name="cancellationToken">キャンセル・トークン</param>
    /// <returns>タスク</returns>
    /// <exception cref="Exception">旅行またはユーザーが見つからない場合にスローされます</exception>
    public async Task Handle(AddMemberCommand request, CancellationToken cancellationToken)
    {
        var trip = await _tripRepository.GetByIdAsync(request.TripId, cancellationToken);
        if (trip == null)
        {
            throw new Exception($"Trip with ID {request.TripId} not found.");
        }

        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (user == null)
        {
            throw new Exception($"User with email {request.Email} not found.");
        }

        var member = TripMember.Create(Guid.NewGuid(), trip.Id, user.Id, request.Role);
        trip.AddMember(member);

        await _tripRepository.UpdateAsync(trip, cancellationToken);
    }
}
