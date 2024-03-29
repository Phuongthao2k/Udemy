using API.DTOs;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API.Extensions;
using AutoMapper;
using API.Entities;

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
    public async Task<ActionResult> UpdateUser(MemberUppdateDto memberUpdateDto)
    {
        var username = User.GetUsername();
        var user = await _userRepository.GetUserByUsernameAsync(username);
        if (user == null) return NotFound();
        _mapper.Map(memberUpdateDto, user);
        _userRepository.Update(user);
        if (await _userRepository.SaveAllAsync()) return NoContent();
        return BadRequest("Failed to update user");
    }

	[HttpPost("add-photo")]
    public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
    {
        var user = await
            _userRepository.GetUserByUsernameAsync(User.GetUsername());

		if (user == null) return NotFound();
       
        var result = await _photoService.AddPhotoAsync(file);
        
        if (result.Error != null) return BadRequest(result.Error.Message);
        
        var photo = new Photo
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId
        };

		if(user.Photos.Count == 0) photo.IsMain = true;
        
        user.Photos.Add(photo);
		
        if (await _userRepository.SaveAllAsync())
        {
            return CreatedAtRoute(nameof(GetUser), 
			new { username = user.UserName }, _mapper.Map<PhotoDto>(photo));
        }
		
        return BadRequest("Problem addding photo");
    }

    [HttpPut("set-main-photo/{photoId}")]
    public async Task<ActionResult> SetMainPhoto(int photoId)
    {
        var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

        var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

        if (photo.IsMain) return BadRequest("This is already your main photo");

        var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
        if (currentMain != null) currentMain.IsMain = false;
        photo.IsMain = true;

        if (await _userRepository.SaveAllAsync()) return NoContent();

        return BadRequest("Failed to set main photo");
    }

    [HttpDelete("delete-photo/{photoId}")]
    public async Task<ActionResult> DeletePhoto(int photoId)
    {
        var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

        var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

        if (photo == null) return NotFound();

        if (photo.IsMain) return BadRequest("You cannot delete your main photo");

        if (photo.PublicId != null)
        {
            var result = await _photoService.DeletePhotoAsync(photo.PublicId);
            if (result.Error != null) return BadRequest(result.Error.Message);
        }

        user.Photos.Remove(photo);

        if (await _userRepository.SaveAllAsync()) return Ok();

        return BadRequest("Failed to delete the photo");
    }
}