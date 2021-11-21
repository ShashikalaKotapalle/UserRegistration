using System.Collections.Generic;
using System.Threading.Tasks;
using UserRegistration.Core.Models.User;

namespace UserRegistration.Core.Interfaces.Repositories
{
	public interface IUserRepository
	{
		Task CreateUser(UserModel user);
		Task CreateUsers(List<UserModel> users);
		Task<UserModel> GetUserByName(string userName);
		Task<UserModel> GetUserById(int userId);
		Task<List<UserModel>> GetAllUsers();
		Task UpdateUser(UserModel user);
		Task DeleteUserById(int userId);
	}
}
