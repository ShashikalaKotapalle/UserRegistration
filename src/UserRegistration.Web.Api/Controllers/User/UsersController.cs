using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using UserRegistration.Core.Interfaces.Services.User;
using UserRegistration.Core.Models.User;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UserRegistration.Web.Api.Controllers.User
{
	[Route("api/users")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly IUserService _userService;
		private readonly ILogger<UsersController> _logger;
		private readonly IMapper _mapper;

		public UsersController(IUserService userService, ILogger<UsersController> logger, IMapper mapper)
		{
			_userService = userService;
			_logger = logger;
			_mapper = mapper;

		}

		// GET: api/<UsersController>
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			try
			{
				var users = await _userService.GetAllUsers();
				if (users != null)
				{
					return Ok(_mapper.Map<List<ResponseDto>>(users));
				}

				return NotFound(UserResources.UsersDoesNotExist);
			}
			catch (Exception e)
			{
				_logger.LogError(e, UserResources.GetUsers_Error);
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
		}

		// GET api/<UsersController>/5
		[HttpGet("{id}")]
		public async Task<IActionResult> Get(int id)
		{
			try
			{
				var user = await _userService.GetUserById(id);
				if (user != null)
				{
					return Ok(_mapper.Map<ResponseDto>(user));
				}

				return NotFound(UserResources.UsersDoesNotExist);
			}
			catch (Exception e)
			{
				_logger.LogError(e, UserResources.GetUsersById_Error);
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
		}

		// POST api/<UsersController>
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] RequestDto dto)
		{
			try
			{
				var userInDb = await _userService.GetUserByName(dto.UserName);
				if (userInDb != null)
				{
					return Conflict();
				}

				UserModel toAddUser = _mapper.Map<UserModel>(dto);
				var createdUser = await _userService.CreateUser(toAddUser);

				return Ok(_mapper.Map<ResponseDto>(createdUser));
			}
			catch (Exception e)
			{
				_logger.LogError(e, UserResources.CreateUserError);
				return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
			}
		}

		// PUT api/<UsersController>/5
		[HttpPut("{id}")]
		public async Task<IActionResult> Put(int id, [FromBody] RequestDto dto)
		{
			try
			{
				var userInDb = await _userService.GetUserByName(dto.UserName);
				if (userInDb != null)
				{
					return Conflict();
				}

				UserModel user = new UserModel
				{
					UserId = id,
					UserName = dto.UserName
				};
				await _userService.UpdateUser(user);

				return Ok(UserResources.UpdateUserSuccess);

				// validate and return No content if User does not exist
			}
			catch (Exception e)
			{
				_logger.LogError(e, UserResources.UpdateUserError);
				return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
			}
		}

		// DELETE api/<UsersController>/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				var userInDb = await _userService.GetUserById(id);
				if (userInDb == null)
				{
					return NotFound(UserResources.UsersDoesNotExist);
				}

				await _userService.DeleteUserById(id);
				return Ok(UserResources.DeleteSuccess);

				// validate and return No content if User does not exist
			}
			catch (Exception e)
			{
				_logger.LogError(e, UserResources.DeleteError);
				return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
			}
		}
	}
}
