using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
	public interface IUserRepository
	{
		Task<bool> SaveAllAsync();

		Task<AppUser> GetUserByIdAsync(int id);

		Task<AppUser> GetUserByUsernameAsync(string username);

		Task<IEnumerable<MemberDto>> GetMembersAsync(UserParams userParams);

		Task<MemberDto> GetMemberAsync(string username);

		void Update(AppUser user);
	}
}