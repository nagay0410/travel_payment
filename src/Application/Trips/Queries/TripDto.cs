using Application.Common.Mappings;

namespace Application.Trips.Queries;

public class TripDto
{
    public Guid Id { get; set; }
    public string TripName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal BudgetAmount { get; set; }
    public string Currency { get; set; } = "JPY";
    public string Status { get; set; } = string.Empty;
    public List<TripMemberDto> Members { get; set; } = new();
}

public class TripMemberDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}
