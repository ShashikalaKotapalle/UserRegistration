using NSubstitute;
using UserRegistration.Core.Interfaces.Persistence;
using UserRegistration.Core.Interfaces.Repositories;
using UserRegistration.Core.Interfaces.Services.Validation;
using UserRegistration.Core.Services.User;

namespace UserRegistration.Core.Services.UnitTests.User.UserServiceTests
{
	public static class UserServiceFactory
	{
		public static UserService Create(
				IUserRepository userRepository,
				IUnitOfWork unitOfWork,
				IValidationService validationService)
			{ 
			return new UserService(
				userRepository ?? Substitute.For<IUserRepository>(),
				unitOfWork ?? Substitute.For<IUnitOfWork>(),
				validationService ?? Substitute.For<IValidationService>());
		}
	}
}
