using API.DTOs;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class UsersController : BaseApiController
{
	private readonly IUserRepository _userRepository;

	public UsersController(IUserRepository userRepository)
	{
		_userRepository = userRepository;
	}

	[AllowAnonymous]
	[HttpGet]
	public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
	{
		var users = await _userRepository.GetMembersAsync();
		return Ok(users);
	}

	[HttpGet("{username}")]
	public async Task<ActionResult<MemberDto>> GetUser(string username)
	{
		var user = await _userRepository.GetMemberAsync(username);
		return user;
	}
}