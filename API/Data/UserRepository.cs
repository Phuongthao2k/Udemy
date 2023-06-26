using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
	public class UserRepository : IUserRepository
	{
		private DataContext _context;

		public UserRepository(DataContext context)
		{
			_context = context;
		}

		public async Task<AppUser> GetUserById(int id)
		{
			return await _context.Users.FindAsync(id);
		}

		public async Task<AppUser> GetUserByUsername(string username)
		{
			return await _context.Users
				.Include(x => x.Photos)
				.SingleOrDefaultAsync(x => x.UserName.Equals(username));
		}

		public async Task<IEnumerable<AppUser>> GetUsers()
		{
			return await _context.Users
				.Include(x => x.Photos)
				.ToListAsync();
		}

		public async Task<bool> SaveAllAsync()
		{
			return await _context.SaveChangesAsync() > 0;
		}

		public void Update(AppUser user)
		{
			_context.Entry(user).State = EntityState.Modified;
		}
	}
}