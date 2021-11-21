using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UserRegistration.Core.Models.User;
using Xunit;

namespace UserRegistration.Infrastructure.Data.IntegrationTests.Repositories.UserRepositoryTests
{
	public class GetUserByIdShould
	{
		[Fact]
		public async void ReturnNull_WhenUserDoesNotExist()
		{
			using (var context = new UserRepositoryTestContext())
			{
				// Arrange
				int userId = 15;
				var sut = context.Sut;

				// Act
				var actual = await sut.GetUserById(userId);

				// Assert
				actual.Should().BeNull();
			}
		}

		[Fact]
		public async void ReturnExpectedValues_WhenUserExists()
		{
			using (var context = new UserRepositoryTestContext())
			{
				// Arrange
				var fixture = new Fixture();
				var user = fixture.Create<UserModel>();
				var sut = context.Sut;

				// Act
				await sut.CreateUser(user);
				await context.UnitOfWork.SaveChangesAsync();

				var userInDb = await context.UserRegistrationDbContext.Users
					.FirstOrDefaultAsync(u => u.UserName == user.UserName);

				var expected = new UserModel
				{
					UserName = user.UserName
				};

				var actual = await sut.GetUserById(userInDb.UserId);

				// Assert
				actual.Should().BeEquivalentTo(expected, us => us
				   .Excluding(u => u.UserId)); // Assigned automatically by DB
			}
		}
	}
}
