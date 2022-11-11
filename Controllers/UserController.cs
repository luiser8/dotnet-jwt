using DotnetJWT.Models;
using DotnetJWT.Request.User.Payloads;
using Microsoft.AspNetCore.Mvc;
using DotnetJWT.Responses;
using DotnetJWT.Services;

namespace DotnetJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(
            IUserService userService
        ){
            _userService = userService;
        }

        /// <summary>Users lists</summary>
        /// <remarks>It is possible to list users created.</remarks>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<UserResponse>> GetUsers()
        {
            try
            {
                var response = await _userService.GetUsers();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>User list by unique ID</summary>
        /// <remarks>It is possible to list a created user.</remarks>
        /// <param name="id">Unique identifier of a user.</param>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<UserResponse>> GetUser(int id)
        {
            try
            {
                var response = await _userService.GetUser(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>Create users</summary>
        /// <remarks>It is possible to create users.</remarks>
        /// <param name="userPayload">Parameters to create a user.</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<User>> CreateUser([FromBody] UserPayload userPayload)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _userService.PostUsers(userPayload);

                return Created("Create", result.Id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>Update a user</summary>
        /// <remarks>It is possible to update users.</remarks>
        /// <param name="id">Unique identifier of a user.</param>
        /// <param name="user">Parameters to update a user.</param>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<User>> PutUser(int id, [FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var request = await _userService.PutUsers(id, user);
                return Ok(request);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>Delete a user</summary>
        /// <remarks>It is possible to delete users.</remarks>
        /// <param name="id">Unique identifier of a user.</param>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            if (id == 0)
            {
                return BadRequest("You must put user ID to delete");
            }
            try
            {
                var request = await _userService.DeleteUsers(id);
                return Ok(request.Id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}