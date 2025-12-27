using Api.Common;
using Application.Payments.Commands.CreatePayment;
using Application.Payments.Queries;
using Application.Payments.Queries.GetPaymentsByTripId;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// 支払い情報の記録・参照を行う API。
/// </summary>
public class PaymentsController : ApiControllerBase
{
    /// <summary>
    /// 新しい支払い記録を追加します。
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreatePaymentCommand command)
    {
        var paymentId = await Mediator.Send(command);
        return Ok(ApiResponse<Guid>.CreateSuccess(paymentId, "Payment recorded successfully"));
    }

    /// <summary>
    /// 指定された旅行に関連する支払い一覧を取得します。
    /// </summary>
    [HttpGet("trip/{tripId}")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<PaymentDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByTripId(Guid tripId)
    {
        var payments = await Mediator.Send(new GetPaymentsByTripIdQuery(tripId));
        return Ok(ApiResponse<IReadOnlyList<PaymentDto>>.CreateSuccess(payments));
    }
}
