using Domain.Entities;
using Domain.ValueObjects;
using Application.Trips.Queries;
using Application.Payments.Queries;
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

        CreateMap<Payment, PaymentDto>()
            .ForMember(d => d.Title, opt => opt.MapFrom(s => s.Description))
            .ForMember(d => d.Amount, opt => opt.MapFrom(s => s.Amount.Amount))
            .ForMember(d => d.Currency, opt => opt.MapFrom(s => s.Amount.Currency));

        CreateMap<PaymentParticipant, PaymentParticipantDto>();
    }
}

public class UserDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
