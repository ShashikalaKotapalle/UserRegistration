using System;
using FluentAssertions;
using UserRegistration.Core.Models.User;
using UserRegistration.Core.Validation.User;
using Xunit;

namespace UserRegistration.Core.Validation.UnitTests.User
{
	public class UserModelValidatorShould
	{

		private readonly UserModelValidator _sut;

		public UserModelValidatorShould()
		{
			_sut = new UserModelValidator();
		}

		[Fact]
		public void ReturnValidResult_WhenExpectedValidValuesAreSe()
		{
			// Arrange
			var model = CreateValidData();

			// Act
			var result = _sut.Validate(model);

			// Assert
			result.IsValid.Should().BeTrue();
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("  ")]
		public void AddUserNameError_WhenUserNameIsNullOrWhitespace(string value)
		{
			// Arrange
			var model = new UserModel
			{
				UserName = ""
			};

			// Act
			var result = _sut.Validate(model);

			// Assert
			result.Errors.Should().Contain(e => e.PropertyName == nameof(model.UserName));
		}

		[Fact]
		public void AddUserNameError_WhenUserNameLengthIsOver100Characters()
		{
			// Arrange
			var model = CreateValidData();

			model.UserName = GenerateByLength(101);

			// Act
			var result = _sut.Validate(model);

			// Assert
			result.Errors.Should().Contain(e => e.PropertyName == nameof(model.UserName));
		}

		private string GenerateByLength(int length)
		{
			Random rd = new Random();
			const string allowedChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789!@$?_-";
			char[] chars = new char[length];

			for (int i = 0; i < length; i++)
			{
				chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
			}

			return new string(chars);
		}

		private UserModel CreateValidData()
		{
			return new UserModel
			{
				UserName = "name"
			};
		}
	}
}
