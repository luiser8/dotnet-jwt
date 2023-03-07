using DotnetJWT.Models;
using DotnetJWT.Request.User.Payloads;
using Microsoft.AspNetCore.Mvc;
using DotnetJWT.Responses;
using DotnetJWT.Services;
using Microsoft.AspNetCore.Authorization;

namespace DotnetJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(
            IUserService userService
        )
        {
            _userService = userService;
        }

        /// <summary>Users list</summary>
        /// <remarks>It is possible return user list.</remarks>
        [HttpGet, Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<UserResponse>> GetUsers()
        {
            try
            {
                var response = await _userService.GetUsersService();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Users login</summary>
        /// <remarks>It is possible user login credentials.</remarks>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<UserResponse>> LoginUser([FromBody] LoginPayload loginPayload)
        {
            try
            {
                var response = await _userService.LoginUserService(loginPayload);
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = response.TokenExpires,
                    Secure = true,
                    IsEssential = true
                };
                Response.Cookies.Append("accessToken", response.AccessToken, cookieOptions);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Create users</summary>
        /// <remarks>It is possible to create users.</remarks>
        /// <param name="userPayload">Parameters to create a user.</param>
        [HttpPost, Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<User>> CreateUser([FromBody] UserPayload userPayload)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var request = await _userService.PostUsersService(userPayload);

                return Created("Create", request);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Refresh Token</summary>
        /// <remarks>It is possible user refresh token credentials.</remarks>
        /// <param name="actualToken">Token actual for refresh.</param>
        [HttpPut("refreshtoken")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<string>> RefreshToken(string actualToken)
        {
            try
            {
                var response = await _userService.RefreshTokenService(actualToken);
                if (response == "Invalid Refresh Token")
                {
                    return Unauthorized(response);
                }
                if (response == "Token expired")
                {
                    return Unauthorized(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}