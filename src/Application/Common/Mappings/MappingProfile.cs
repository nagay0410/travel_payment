using Domain.Entities;
using Domain.ValueObjects;
using Application.Trips.Queries;
using Application.Users.Commands.CreateUser; // UserDto はここにある場合があるが、Common に移動したか確認が必要

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

        CreateMap<Trip, TripDto>()
            .ForMember(d => d.BudgetAmount, opt => opt.MapFrom(s => s.Budget != null ? s.Budget.Amount : 0))
            .ForMember(d => d.Currency, opt => opt.MapFrom(s => s.Budget != null ? s.Budget.Currency : "JPY"));

        CreateMap<TripMember, TripMemberDto>();
    }
}

public class UserDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
