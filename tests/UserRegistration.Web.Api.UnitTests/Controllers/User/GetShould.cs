using System;
using System.Collections.Generic;
using System.Linq;
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
	public class GetShould
	{
		private readonly IUserService _userService;
		private readonly ILogger<UsersController> _logger;
		private readonly IMapper _mapper;
		private readonly Fixture _fixture;

		private readonly UsersController _sut;

		public GetShould()
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
			var users = _fixture.Create<List<UserModel>>();

			var dto = _fixture.Create<List<ResponseDto>>();

			_userService.GetAllUsers().Returns(users);
			_mapper.Map<List<ResponseDto>>(users).Returns(dto);

			//Act
			var result = await _sut.Get();

			//Assert
			result.Should()
				.BeAssignableTo<OkObjectResult>()
				.Which
				.Value
				.Should()
				.BeEquivalentTo(dto);
		}

		[Fact]
		public async void CallService_ToGetAllUsers()
		{
			//Arrange
			var users = _fixture.Create<List<UserModel>>();

			var dto = _fixture.Create<List<ResponseDto>>();

			_userService.GetAllUsers().Returns(users);
			_mapper.Map<List<ResponseDto>>(users).Returns(dto);

			//Act
			await _sut.Get();

			//Assert
			await _userService.Received(1).GetAllUsers();
		}

		[Fact]
		public async void ReturnNotFoundWhen_NoUsersExist()
		{
			//Arrange
			List<UserModel> users = null;

			_userService.GetAllUsers().Returns(users);

			//Act
			var result = await _sut.Get();

			//Assert
			result.Should()
				.BeAssignableTo<NotFoundObjectResult>()
				.Which
				.Value
				.Should()
				.BeEquivalentTo(UserResources.UsersDoesNotExist);
		}

		[Fact]
		public async void ReturnInternalServerErrorWhen_AnExceptionOccurs()
		{
			//Arrange
			var exception = _fixture.Create<Exception>();

			_userService.GetAllUsers().Throws(exception);

			//Act
			var result = await _sut.Get();

			//Assert
			result.Should()
				.BeAssignableTo<StatusCodeResult>()
				.Which
				.StatusCode
				.Should()
				.Be(StatusCodes.Status500InternalServerError);
		}

		[Fact]
		public async void LogErrorWhen_AnExceptionOccurs()
		{
			// Arrange
			string expectedMessage = UserResources.GetUsers_Error;
			var exception = _fixture.Create<Exception>();

			_userService.GetAllUsers().Throws(exception);

			//Act
			await _sut.Get();

			// Assert
			_logger.Received().LogError(Arg.Any<EventId>(), exception, expectedMessage);
		}
	}
}
