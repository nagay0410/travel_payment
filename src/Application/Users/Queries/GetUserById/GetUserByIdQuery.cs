using Application.Common.Mappings;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.Users.Queries.GetUserById;

/// <summary>
/// IDを指定してユーザー情報を取得するクエリ。
/// </summary>
/// <param name="Id">ユーザーID</param>
public record GetUserByIdQuery(Guid Id) : IRequest<UserDto?>;

/// <summary>
/// <see cref="GetUserByIdQuery"/> を処理するハンドラー。
/// </summary>
public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto?>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// 新しいインスタンスを生成します。
    /// </summary>
    /// <param name="userRepository">ユーザーリポジトリ</param>
    /// <param name="mapper">マッパー</param>
    public GetUserByIdQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// ユーザー情報の取得を実行します。
    /// </summary>
    /// <param name="request">クエリリクエスト</param>
    /// <param name="cancellationToken">キャンセル・トークン</param>
    /// <returns>ユーザー情報（存在しない場合はnull）</returns>
    public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
        
        return user == null ? null : _mapper.Map<UserDto>(user);
    }
}
