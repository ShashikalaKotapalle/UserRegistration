using System.Collections.Generic;
using System.Threading.Tasks;
using UserRegistration.Core.Models.User;

namespace UserRegistration.Core.Interfaces.Services.User
{
	public interface IUserService
	{
		Task<UserModel> CreateUser(UserModel userModel);
		Task<UserModel> GetUserById(int userId);
		Task<List<UserModel>> GetAllUsers();
		Task UpdateUser(UserModel user);
		Task DeleteUserById(int userId);
		Task<UserModel> GetUserByName(string userName);
	}
}
