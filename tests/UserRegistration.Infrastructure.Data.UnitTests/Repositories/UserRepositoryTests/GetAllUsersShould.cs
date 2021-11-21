using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using UserRegistration.Core.Models.User;
using Xunit;

namespace UserRegistration.Infrastructure.Data.IntegrationTests.Repositories.UserRepositoryTests
{
	public class GetAllUsersShould
	{
		[Fact]
		public async void ReturnExpectedValues_WhenCalled()
		{
			using (var context = new UserRepositoryTestContext())
			{
				// Arrange
				var fixture = new Fixture();
				var users = fixture.Create<List<UserModel>>();
				var sut = context.Sut;

				// Act
				
				await sut.CreateUsers(users);
				await context.UnitOfWork.SaveChangesAsync();

				var expected = users.Select(u => new UserModel
				{
					UserName = u.UserName
				}).ToList();

				var actual = await sut.GetAllUsers();

				// Assert
				actual.Count.Should().Equals(expected.Count);
			}
		}
	}
}
