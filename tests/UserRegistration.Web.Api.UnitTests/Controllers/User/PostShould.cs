using System;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using FluentAssertions.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using UserRegistration.Core.Interfaces.Services.User;
using UserRegistration.Core.Models.User;
using UserRegistration.Web.Api.Controllers.User;
using UserRegistration.Web.Api.UnitTests.Helpers;
using Xunit;

namespace UserRegistration.Web.Api.UnitTests.Controllers.User
{
	public class PostShould
	{
		private readonly IUserService _userService;
		private readonly ILogger<UsersController> _logger;
		private readonly IMapper _mapper;
		private readonly Fixture _fixture;

		private readonly UsersController _sut;

		public PostShould()
		{
			_logger = Substitute.For<ILogger<UsersController>>();
			_userService = Substitute.For<IUserService>();
			_mapper = Substitute.For<IMapper>();
			_fixture = AutoFixtureFactory.CreateFixture();

			_sut = UsersControllerFactory.Create(_userService, _logger, _mapper);
		}

		[Fact]
		public async void ReturnOKWhen_NoExceptionThrown()
		{
			//Arrange
			var user = _fixture.Create<UserModel>();
			var dto = _fixture.Create<RequestDto>();

			UserModel userInDb = null;
			_userService.GetUserByName(dto.UserName).Returns(userInDb);
			_mapper.Map<UserModel>(dto).Returns(user);
			_userService.CreateUser(user).Returns(user);

			var dtoToReturn = _fixture.Create<ResponseDto>();
			_mapper.Map<ResponseDto>(user).Returns(dtoToReturn);

			//Act
			var result = await _sut.Post(dto);

			//Assert
			result.Should()
				.BeAssignableTo<OkObjectResult>()
				.Which
				.Value
				.Should()
				.BeEquivalentTo(dtoToReturn);
		}

		[Fact]
		public async void ReturnConflictWhen_UserExists()
		{
			//Arrange
			var user = _fixture.Create<UserModel>();
			var dto = _fixture.Create<RequestDto>();

			_userService.GetUserByName(dto.UserName).Returns(user);
			_mapper.Map<UserModel>(dto).Returns(user);
			_userService.CreateUser(user).Returns(user);

			var dtoToReturn = _fixture.Create<ResponseDto>();
			_mapper.Map<ResponseDto>(user).Returns(dtoToReturn);

			//Act
			var result = await _sut.Post(dto);

			//Assert
			result.Should()
				.BeAssignableTo<ConflictResult>();

		}

		[Fact]
		public async void CallService_ToGetAllUsers()
		{
			//Arrange
			var user = _fixture.Create<UserModel>();
			var dto = _fixture.Create<RequestDto>();
			UserModel userInDb = null;
			_userService.GetUserByName(dto.UserName).Returns(userInDb);

			_mapper.Map<UserModel>(dto).Returns(user);
			_userService.CreateUser(user).Returns(user);

			var dtoToReturn = _fixture.Create<ResponseDto>();
			_mapper.Map<ResponseDto>(user).Returns(dtoToReturn);

			//Act
			var result = await _sut.Post(dto);

			//Assert
			await _userService.Received(1).CreateUser(user);
		}

		[Fact]
		public async void ReturnInternalServerErrorWhen_AnExceptionOccurs()
		{
			//Arrange
			var exception = _fixture.Create<Exception>();
			var user = _fixture.Create<UserModel>();
			var dto = _fixture.Create<RequestDto>();
			UserModel userInDb = null;
			_userService.GetUserByName(dto.UserName).Returns(userInDb);
			_mapper.Map<UserModel>(dto).Returns(user);

			_userService.CreateUser(user).Throws(exception);

			//Act
			var result = await _sut.Post(dto);

			//Assert
			result.Should()
				.BeAssignableTo<ObjectResult>()
				.Which
				.StatusCode
				.Should()
				.Be(StatusCodes.Status500InternalServerError);
		}

		[Fact]
		public async void LogErrorWhen_AnExceptionOccurs()
		{
			// Arrange
			string expectedMessage = UserResources.CreateUserError;
			var exception = _fixture.Create<Exception>();
			var user = _fixture.Create<UserModel>();
			var dto = _fixture.Create<RequestDto>();
			_mapper.Map<UserModel>(dto).Returns(user);

			_userService.CreateUser(user).Throws(exception);

			//Act
			var result = await _sut.Post(dto);

			// Assert
			_logger.Received().LogError(Arg.Any<EventId>(), exception, expectedMessage);
		}
	}
}
