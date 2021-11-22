using System.Collections.Generic;
using System.Threading.Tasks;
using UserRegistration.Core.Interfaces.Persistence;
using UserRegistration.Core.Interfaces.Repositories;
using UserRegistration.Core.Interfaces.Services.User;
using UserRegistration.Core.Interfaces.Services.Validation;
using UserRegistration.Core.Models.User;
using UserRegistration.Core.Models.Validation;

namespace UserRegistration.Core.Services.User
{
	public class UserService : IUserService
	{
		private readonly IUserRepository _userRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IValidationService _validationService;

		public UserService(
			IUserRepository userRepository,
			IUnitOfWork unitOfWork,
			IValidationService validationService)
		{
			_userRepository = userRepository;
			_unitOfWork = unitOfWork;
			_validationService = validationService;
		}

		public async Task<UserModel> CreateUser(UserModel userModel)
		{
			var validation =  await _validationService.ValidateAsync(userModel);
			if (!validation.IsValid)
			{
				throw new ValidationFailedException(validation.Errors);
			}
			var user = await _userRepository.GetUserByName(userModel.UserName);
			if (user != null)
			{
				var error = new ValidationFailureModel(nameof(UserModel.UserName),
					UserServiceResources.UserExists);
				throw new ValidationFailedException(new List<ValidationFailureModel> { error });
			}

			await _userRepository.CreateUser(userModel);
			await _unitOfWork.SaveChangesAsync();

			return await _userRepository.GetUserByName(userModel.UserName);

			// Auditing
		}

		public async Task<UserModel> GetUserById(int userId)
		{
			return await _userRepository.GetUserById(userId);
		}

		public async Task<List<UserModel>> GetAllUsers()
		{
			return await _userRepository.GetAllUsers();
		}

		public async Task UpdateUser(UserModel user)
		{
			var validation = await _validationService.ValidateAsync(user);
			if (!validation.IsValid)
			{
				throw new ValidationFailedException(validation.Errors);
			}
			
			await _userRepository.UpdateUser(user);

			await _unitOfWork.SaveChangesAsync();

			// Auditing
		}

		public async Task DeleteUserById(int userId)
		{
			await _userRepository.DeleteUserById(userId);

			await _unitOfWork.SaveChangesAsync();

			// Auditing
		}

		public async Task<UserModel> GetUserByName(string userName)
		{
			return await _userRepository.GetUserByName(userName);
		}
	}
}
