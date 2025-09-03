using Microsoft.AspNetCore.Mvc;
using TaskTracker.Application.DTOs;
using TaskTracker.Application.Interfaces;

namespace TaskTracker.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class UsersController : ControllerBase
	{
		private readonly IUserService _userService;

		public UsersController(IUserService userService)
		{
			_userService = userService;
		}

		/// <summary>
		/// Регистрация пользователя.
		/// </summary>
		/// <param name="registerRequest">Регистрационные данные.</param>
		/// <returns>Идентификатор пользователя.</returns>
		/// <response code="200">Пользователь успешно зарегистрирован.</response>
		/// <response code="409">Ошибка - Email уже зарегистрирован.</response>
		[HttpPost("Register")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status409Conflict)]
		public async Task<ActionResult> Register(UserRegisterRequest registerRequest)
		{
			var userId = await _userService.Register(registerRequest);

			if(userId == Guid.Empty)
			{
				return Conflict(new
				{
					error = "Email already exists.",
					message = "This provided email is already registered."
				});
			}

			return Ok(userId);
		}

		/// <summary>
		/// Войти в учётную запись.
		/// </summary>
		/// <param name="loginRequest">Данные для аутентификации.</param>
		/// <response code="200">Пользователь успешно вошёл в учётную запись.</response>
		/// <response code="404">Ошибка - Неверный Email или пароль.</response>
		[HttpPost("Login")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> Login(UserLoginRequest loginRequest)
		{
			var jwtToken = await _userService.Login(loginRequest);

			HttpContext.Response.Cookies.Append("token", jwtToken);

			return Ok();
		}
	}
}
