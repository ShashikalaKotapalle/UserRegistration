using AutoFixture;
using FluentAssertions;
using NSubstitute;
using UserRegistration.Core.Interfaces.Persistence;
using UserRegistration.Core.Interfaces.Repositories;
using UserRegistration.Core.Interfaces.Services.User;
using UserRegistration.Core.Interfaces.Services.Validation;
using UserRegistration.Core.Models.User;
using Xunit;

namespace UserRegistration.Core.Services.UnitTests.User.UserServiceTests
{
	public class DeleteUserByIdShould
	{
		private readonly IUserRepository _userRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IValidationService _validationService;
		private readonly Fixture _fixture;

		private readonly IUserService _sut;

		public DeleteUserByIdShould()
		{
			_userRepository = Substitute.For<IUserRepository>();
			_unitOfWork = Substitute.For<IUnitOfWork>();
			_validationService = Substitute.For<IValidationService>();
			_fixture = new Fixture();
			_sut = UserServiceFactory.Create(_userRepository, _unitOfWork, _validationService);

		}

		[Fact]
		public async void DeleteTheUser_WhenCalled()
		{
			// Arrange
			int id = 1;

			// Act
			await _sut.DeleteUserById(id);

			// Assert
			await _userRepository.Received(1).DeleteUserById(id);
		}

		[Fact]
		public async void SaveTheChanges_WhenCalled()
		{
			// Arrange
			int id = 1;

			// Act
			await _sut.DeleteUserById(id);

			// Assert
			await _unitOfWork.Received(1).SaveChangesAsync();
		}
	}
}
