using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
	public class UserRepository : IUserRepository
	{
		private DataContext _context;
		private readonly IMapper _mapper;

		public UserRepository(DataContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<MemberDto> GetMemberAsync(string username)
		{
			return await _context.Users
							.Where(u => u.UserName == username)
							.ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
							.SingleOrDefaultAsync();
		}

		public async Task<IEnumerable<MemberDto>> GetMembersAsync()
		{
			return await _context.Users
							.ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
							.ToListAsync();
		}

		//public async Task<AppUser> GetUserById(int id)
		//{
		//	return await _context.Users.FindAsync(id);
		//}
	}
}