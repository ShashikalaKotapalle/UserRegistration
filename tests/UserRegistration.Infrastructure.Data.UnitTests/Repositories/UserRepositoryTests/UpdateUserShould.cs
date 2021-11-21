using AutoFixture;
using FluentAssertions;
using UserRegistration.Core.Models.User;
using Xunit;

namespace UserRegistration.Infrastructure.Data.IntegrationTests.Repositories.UserRepositoryTests
{
	public class UpdateUserShould
	{
		[Fact]
		public async void UpdateUserWithExpectedValues_WhenCalledAndSaved()
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
				var createdUser = await sut.GetUserByName(user.UserName);

				createdUser.UserName = "Newename";
				await sut.UpdateUser(createdUser);
				await context.UnitOfWork.SaveChangesAsync();

				var expected = new UserModel
				{
					UserId = createdUser.UserId,
					UserName = createdUser.UserName
				};

				var actual = await sut.GetUserByName(createdUser.UserName);

				// Assert
				actual.Should().BeEquivalentTo(expected);
			}
		}
    }
}
