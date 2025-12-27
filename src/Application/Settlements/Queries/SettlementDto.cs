namespace Application.Settlements.Queries;

public class SettlementDto
{
    public Guid FromUserId { get; set; }
    public string FromUserName { get; set; } = string.Empty;
    public Guid ToUserId { get; set; }
    public string ToUserName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "JPY";
}
