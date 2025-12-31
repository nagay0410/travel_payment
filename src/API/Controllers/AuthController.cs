using Api.Common;
using Application.Auth.Commands.Login;
using Application.Auth.Common;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// ログイン認証を行うAPI。
/// </summary>
public class AuthController : ApiControllerBase
{
    /// <summary>
    /// ログイン認証を行います。
    /// </summary>
    /// <param name="command">認証情報</param>
    /// <returns>認証結果（JWTトークンを含む）</returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<AuthenticationResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        try
        {
            var result = await Mediator.Send(command);
            return Ok(ApiResponse<AuthenticationResult>.CreateSuccess(result, "ログインに成功しました。"));
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ApiResponse<object>.CreateFailure(new List<string> { ex.Message }, "認証に失敗しました。"));
        }
    }
}

