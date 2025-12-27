using Api.Common;
using Application.Common.Mappings;
using Application.Users.Commands.CreateUser;
using Application.Users.Queries.GetUserById;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// ユーザー情報の管理を行う API。
/// </summary>
public class UsersController : ApiControllerBase
{
    /// <summary>
    /// 新しいユーザーを登録します。
    /// </summary>
    /// <param name="command">ユーザー登録情報</param>
    /// <returns>作成されたユーザーのID</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
    {
        var userId = await Mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = userId }, ApiResponse<Guid>.CreateSuccess(userId, "User created successfully"));
    }

    /// <summary>
    /// 指定されたIDのユーザー情報を取得します。
    /// </summary>
    /// <param name="id">ユーザーID</param>
    /// <returns>ユーザー情報</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await Mediator.Send(new GetUserByIdQuery(id));
        if (user == null) return NotFound(ApiResponse<object>.CreateFailure(new List<string> { "User not found" }, "Not Found"));

        return Ok(ApiResponse<UserDto>.CreateSuccess(user));
    }
}
