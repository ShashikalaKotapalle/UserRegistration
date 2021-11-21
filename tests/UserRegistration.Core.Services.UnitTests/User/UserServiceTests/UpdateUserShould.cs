using System.Collections.Generic;
using AutoFixture;
using FluentAssertions;
using NSubstitute;
using UserRegistration.Core.Interfaces.Persistence;
using UserRegistration.Core.Interfaces.Repositories;
using UserRegistration.Core.Interfaces.Services.User;
using UserRegistration.Core.Interfaces.Services.Validation;
using UserRegistration.Core.Models.User;
using UserRegistration.Core.Models.Validation;
using Xunit;

namespace UserRegistration.Core.Services.UnitTests.User.UserServiceTests
{
	public class UpdateUserShould
	{
		private readonly IUserRepository _userRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IValidationService _validationService;
		private readonly Fixture _fixture;

		private readonly IUserService _sut;

		public UpdateUserShould()
		{
			_userRepository = Substitute.For<IUserRepository>();
			_unitOfWork = Substitute.For<IUnitOfWork>();
			_validationService = Substitute.For<IValidationService>();
			_fixture = new Fixture();
			_sut = UserServiceFactory.Create(_userRepository, _unitOfWork, _validationService);

		}

		[Fact]
		public async void CallValidateAsync_WhenCalled()
		{
			// Arrange
			var user = _fixture.Create<UserModel>();

			ValidationResultModel result = new ValidationResultModel();

			_validationService.ValidateAsync(user).Returns(result);

			// Act
			await _sut.UpdateUser(user);

			// Assert
			await _validationService.Received(1)
				.ValidateAsync(user);
		}

		[Fact]
		public async void NotCallRepository_WhenCallValidateAsync_ExceptionThrown()
		{
			// Arrange
			var user = _fixture.Create<UserModel>();
			List<ValidationFailureModel> failureModels = _fixture.Create<List<ValidationFailureModel>>();
			ValidationResultModel result = new ValidationResultModel(failureModels);
			
			_validationService.ValidateAsync(user).Returns(result);

			// Act
			var exception = await Record.ExceptionAsync(async () => { await _sut.UpdateUser(user); });

			// Assert
			await _userRepository.DidNotReceive().UpdateUser(user);
		}

		[Fact]
		public async void ThrowValidationFailedException_WhenValidationFails()
		{
			// Arrange
			var user = _fixture.Create<UserModel>();
			List<ValidationFailureModel> failureModels = _fixture.Create<List<ValidationFailureModel>>();
			ValidationResultModel result = new ValidationResultModel(failureModels);

			_validationService.ValidateAsync(user).Returns(result);

			// Act
			var exception = await Record.ExceptionAsync(async () => { await _sut.UpdateUser(user); });

			// Assert
			exception.Should()
				.BeAssignableTo<ValidationFailedException>();

		}

		[Fact]
		public async void UpdateTheUser_WhenCalled()
		{
			// Arrange
			var user = _fixture.Create<UserModel>();
			
			ValidationResultModel result = new ValidationResultModel();

			_validationService.ValidateAsync(user).Returns(result);


			// Act
			await _sut.UpdateUser(user);

			// Assert
			await _userRepository.Received(1).UpdateUser(user);
		}

		[Fact]
		public async void SaveTheChanges_WhenCalled()
		{
			// Arrange
			var user = _fixture.Create<UserModel>();
			ValidationResultModel result = new ValidationResultModel();

			_validationService.ValidateAsync(user).Returns(result);

			// Act
			await _sut.UpdateUser(user);
			
			// Assert
			await _unitOfWork.Received(1).SaveChangesAsync();
		}

	}
}
