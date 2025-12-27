namespace Application.Payments.Queries;

public class PaymentDto
{
    public Guid Id { get; set; }
    public Guid TripId { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "JPY";
    public DateTime PaymentDate { get; set; }
    public List<PaymentParticipantDto> Participants { get; set; } = new();
}

public class PaymentParticipantDto
{
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
}
