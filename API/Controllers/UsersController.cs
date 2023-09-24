using API.DTOs;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers;

[Authorize]
public class UsersController : BaseApiController
{
	private readonly IUserRepository _userRepository;
	private readonly IMapper _mapper;
    private readonly IPhotoService _photoService;

	public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
	{
		_userRepository = userRepository;
		_mapper = mapper;
		_photoService = photoService;
	}

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

	[HttpPut]
	public async Task<ActionResult> UpdateUser(MemberUppdateDto memberUppdateDto)
	{
		var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
		if(user == null) return NotFound();
		_mapper.Map(memberUppdateDto, user);
		_userRepository.Update(user);
		if(await _userRepository.SaveAllAsync()) return NoContent();
		return BadRequest("Failed to update user");
	}
	
}