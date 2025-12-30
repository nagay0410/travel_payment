using Api.Common;
using Application.Trips.Commands.AddMember;
using Application.Trips.Commands.CreateTrip;
using Application.Trips.Queries;
using Application.Trips.Queries.GetTripById;
using Application.Trips.Queries.GetTripsByUserId;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// 旅行情報の管理を行う API。
/// </summary>
public class TripsController : ApiControllerBase
{
    /// <summary>
    /// 新しい旅行を計画します。
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateTripCommand command)
    {
        var tripId = await Mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = tripId }, ApiResponse<Guid>.CreateSuccess(tripId, "Trip created successfully"));
    }

    /// <summary>
    /// 指定されたIDの旅行詳細を取得します。
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<TripDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var trip = await Mediator.Send(new GetTripByIdQuery(id));
        if (trip == null) return NotFound(ApiResponse<object>.CreateFailure(new List<string> { "Trip not found" }, "Not Found"));
        return Ok(ApiResponse<TripDto>.CreateSuccess(trip));
    }

    /// <summary>
    /// ユーザーが参加している旅行一覧を取得します。
    /// </summary>
    [HttpGet("user/{userId}")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<TripDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByUserId(Guid userId)
    {
        var trips = await Mediator.Send(new GetTripsByUserIdQuery(userId));
        return Ok(ApiResponse<IReadOnlyList<TripDto>>.CreateSuccess(trips));
    }

    /// <summary>
    /// 旅行に新しいメンバーを追加します。
    /// </summary>
    [HttpPost("{id}/members")]
    [ProducesResponseType(typeof(ApiResponse<Unit>), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddMember(Guid id, [FromBody] AddMemberCommand command)
    {
        // ルートパラメータのIDとコマンドのIDが一致することを確認（簡易的）
        if (id != command.TripId) return BadRequest(ApiResponse<object>.CreateFailure(new List<string> { "TripId mismatch" }));

        await Mediator.Send(command);
        return Ok(ApiResponse<Unit>.CreateSuccess(Unit.Value, "Member added successfully"));
    }
}
