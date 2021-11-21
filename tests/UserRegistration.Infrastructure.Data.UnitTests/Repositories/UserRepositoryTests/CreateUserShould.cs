using AutoFixture;
using FluentAssertions;
using UserRegistration.Core.Models.User;
using Xunit;

namespace UserRegistration.Infrastructure.Data.IntegrationTests.Repositories.UserRepositoryTests
{
	public class CreateUserShould
	{
		[Fact]
		public async void AddUserToDatabase_WhenCalledAndSaved()
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

				var expected = new UserModel
				{
					UserName = user.UserName
				};

				var actual = await sut.GetUserByName(user.UserName);

				// Assert
				actual.Should().BeEquivalentTo(expected, us => us
					.Excluding(u => u.UserId)); // Assigned automatically by DB
			}
		}
	}
}
