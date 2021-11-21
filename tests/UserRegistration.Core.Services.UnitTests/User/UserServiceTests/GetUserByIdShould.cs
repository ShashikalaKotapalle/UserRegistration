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
	public class GetUserByIdShould
	{
		private readonly IUserRepository _userRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IValidationService _validationService;
		private readonly Fixture _fixture;

		private readonly IUserService _sut;

		public GetUserByIdShould()
		{
			_userRepository = Substitute.For<IUserRepository>();
			_unitOfWork = Substitute.For<IUnitOfWork>();
			_validationService = Substitute.For<IValidationService>();
			_fixture = new Fixture();
			_sut = UserServiceFactory.Create(_userRepository,_unitOfWork, _validationService);

		}

		[Fact]
		public async void ReturnTheUser_WhenCalled()
		{
			// Arrange
			int id = 1;
			UserModel expected = _fixture.Build<UserModel>()
				.With(u => u.UserId, id)
				.Create();

			_userRepository.GetUserById(id).Returns(expected);

			// Act

			var actual = await _sut.GetUserById(id);

			// Assert
			actual.Should().BeEquivalentTo(expected);
		}

		[Fact]
		public async void CallTheRepository_ToGetTheUser()
		{
			// Arrange
			int id = 1;
			UserModel expected = _fixture.Build<UserModel>()
				.With(u => u.UserId, id)
				.Create();

			_userRepository.GetUserById(id).Returns(expected);

			// Act

			var actual = await _sut.GetUserById(id);

			// Assert
			actual.Should().BeEquivalentTo(expected);

			await _userRepository.Received(1).GetUserById(id);
		}
	}
}
