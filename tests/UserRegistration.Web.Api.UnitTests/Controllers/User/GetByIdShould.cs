using System;
using System.Collections.Generic;
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
	public class GetByIdShould
	{
		private readonly IUserService _userService;
		private readonly ILogger<UsersController> _logger;
		private readonly IMapper _mapper;
		private readonly Fixture _fixture;

		private readonly UsersController _sut;
		private const int _id = 1;

		public GetByIdShould()
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

			var dto = _fixture.Create<UsersDto>();

			_userService.GetUserById(_id).Returns(user);
			_mapper.Map<UsersDto>(user).Returns(dto);

			//Act
			var result = await _sut.Get(_id);

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
			var user = _fixture.Create<UserModel>();

			var dto = _fixture.Create<UsersDto>();

			_userService.GetUserById(_id).Returns(user);
			_mapper.Map<UsersDto>(user).Returns(dto);

			//Act
			var result = await _sut.Get(_id);

			//Assert
			await _userService.Received(1).GetUserById(_id);
		}

		[Fact]
		public async void ReturnNoContentWhen_NoUsersExist()
		{
			//Arrange
			UserModel user = null;

			_userService.GetUserById(_id).Returns(user);

			//Act
			var result = await _sut.Get(_id);

			//Assert
			result.Should()
				.BeAssignableTo<ObjectResult>()
				.Which
				.Value
				.Should()
				.BeEquivalentTo(UserResources.UsersDoesNotExist);

			result.Should()
				.BeAssignableTo<ObjectResult>()
				.Which
				.StatusCode
				.Should()
				.Be(StatusCodes.Status204NoContent);
		}


		[Fact]
		public async void ReturnInternalServerErrorWhen_AnExceptionOccurs()
		{
			//Arrange
			var exception = _fixture.Create<Exception>();

			_userService.GetUserById(_id).Throws(exception);

			//Act
			var result = await _sut.Get(_id);

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
			string expectedMessage = UserResources.GetUsersById_Error;
			var exception = _fixture.Create<Exception>();

			_userService.GetUserById(_id).Throws(exception);

			//Act
			await _sut.Get(_id);

			// Assert
			_logger.Received().LogError(Arg.Any<EventId>(), exception, expectedMessage);

		}
	}
}
