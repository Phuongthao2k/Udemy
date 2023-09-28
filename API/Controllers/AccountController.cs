using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
	public class AccountController : BaseApiController
	{
		private readonly DataContext _context;
		private readonly ITokenService _tokenService;

		public AccountController(DataContext context, ITokenService tokenService)
		{
			_context = context;
			_tokenService = tokenService;
		}

		[HttpPost("register")] // POST: api/account/Register?username=sam&password=password
		public async Task<ActionResult<UserDto>> Register(LoginDto loginDto)
		{
			if (await UserExists(loginDto.Username)) return BadRequest("Username is taken");
			using var hmac = new HMACSHA512();
			var user = new AppUser
			{
				UserName = loginDto.Username.ToLower(),
				PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password)),
				PasswordSalt = hmac.Key
			};

			Console.Write(user);
			_context.Users.Add(user);
			await _context.SaveChangesAsync();

			return new UserDto
			{
				Username = user.UserName,
				Token = _tokenService.CreateToken(user)
			};
		}

		private async Task<bool> UserExists(string username)
		{
			return await _context.Users.AnyAsync(x => x.UserName == username);
		}

		[HttpGet("login")]
		public async Task<ActionResult<UserDto>> Login()
		{
			LoginDto loginDto = new LoginDto { Username = "agnes", Password = "agnes" };
			var users = await _context.Users.ToListAsync();
			var user = await _context.Users.SingleOrDefaultAsync(x =>
				x.UserName == loginDto.Username);

			if (user == null) return Unauthorized("invalid username");

			using var hmac = new HMACSHA512();

			var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
			Console.WriteLine(computeHash);
			for (int i = 0; i < computeHash.Length; i++)
			{
				if (computeHash[i] != user.PasswordHash[i]) return Unauthorized(user.PasswordHash);
			}

			return new UserDto
			{
				Username = user.UserName,
				Token = _tokenService.CreateToken(user)
			};
		}

		[HttpPost("login")]
		public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
		{
			var user = await _context.Users
            .Include(p => p.Photos)
            .SingleOrDefaultAsync(x => x.UserName == loginDto.Username);

			if (user == null) return Unauthorized("invalid username");

			using var hmac = new HMACSHA512(user.PasswordSalt);
			var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
			for (int i = 0; i < computeHash.Length; i++)
			{
				if (computeHash[i] != user.PasswordHash[i]) return Unauthorized("invalid password");
			}

			return new UserDto
			{
				Username = user.UserName,
				Token = _tokenService.CreateToken(user),
				PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url
			};
		}
	}
}