using Domain.Entities;
using Domain.ValueObjects;

namespace Application.Common.Mappings;

/// <summary>
/// 各ドメインエンティティとDTOのマッピング。
/// </summary>
public class MappingProfile : AutoMapper.Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(d => d.Email, opt => opt.MapFrom(s => s.Email.Value));
    }
}

public class UserDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
