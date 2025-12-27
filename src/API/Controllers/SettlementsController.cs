using Api.Common;
using Application.Settlements.Commands.CompleteSettlement;
using Application.Settlements.Queries;
using Application.Settlements.Queries.CalculateSettlements;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace Api.Controllers;

/// <summary>
/// 精算の算出および完了を管理する API。
/// </summary>
public class SettlementsController : ApiControllerBase
{
    /// <summary>
    /// 旅行の精算（誰が誰にいくら払うか）を算出します。
    /// </summary>
    [HttpGet("trip/{tripId}/calculate")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<SettlementDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Calculate(Guid tripId)
    {
        var settlements = await Mediator.Send(new CalculateSettlementsQuery(tripId));
        return Ok(ApiResponse<IReadOnlyList<SettlementDto>>.CreateSuccess(settlements));
    }

    /// <summary>
    /// 精算（支払い）を完了として記録します。
    /// </summary>
    [HttpPost("complete")]
    [ProducesResponseType(typeof(ApiResponse<Unit>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Complete([FromBody] CompleteSettlementCommand command)
    {
        await Mediator.Send(command);
        return Ok(ApiResponse<Unit>.CreateSuccess(Unit.Value, "Settlement completed successfully"));
    }
}
