using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using UserRegistration.Core.Interfaces.Repositories;
using UserRegistration.Core.Models.User;
using UserRegistration.Infrastructure.Data.Entities;
using UserRegistration.Infrastructure.Data.Persistence;

namespace UserRegistration.Infrastructure.Data.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly UserRegistrationDbContext _context;
		private readonly IMapper _mapper;

		public UserRepository(UserRegistrationDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task CreateUser(UserModel user)
		{
			var toAdd = _mapper.Map<User>(user);
			await _context.AddAsync(toAdd);
		}

		public async Task CreateUsers(List<UserModel> users)
		{
			var toAdd = _mapper.Map<List<User>>(users);
			await _context.AddRangeAsync(toAdd);
		}

		public Task<UserModel> GetUserByName(string userName)
		{
			return _context.Users.Where(u => u.UserName == userName)
				.ProjectTo<UserModel>(_mapper.ConfigurationProvider)
				.SingleOrDefaultAsync();
		}

		public Task<UserModel> GetUserById(int userId)
		{
			return _context.Users.Where(u => u.UserId == userId)
				.ProjectTo<UserModel>(_mapper.ConfigurationProvider)
				.SingleOrDefaultAsync();
		}

		public async Task<List<UserModel>> GetAllUsers()
		{
			return await _context.Users.OrderBy(u => u.UserId)
				.ProjectTo<UserModel>(_mapper.ConfigurationProvider)
				.ToListAsync();
		}

		public async Task UpdateUser(UserModel user)
		{
			var toUpdate = await _context.Users.SingleOrDefaultAsync(u => u.UserId == user.UserId);
			
			if (toUpdate == null)
			{
				return;
			}

			toUpdate.UserName = user.UserName;

			_context.Users.Update(toUpdate);
		}

		public async Task DeleteUserById(int userId)
		{
			var toDelete = await _context.Users.SingleOrDefaultAsync(u => u.UserId == userId);

			if (toDelete == null)
			{
				return;
			}

			_context.Users.Remove(toDelete);
		}
	}
}
