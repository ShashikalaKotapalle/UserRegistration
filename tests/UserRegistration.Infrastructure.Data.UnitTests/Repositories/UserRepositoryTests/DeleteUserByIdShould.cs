using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using UserRegistration.Core.Models.User;
using Xunit;

namespace UserRegistration.Infrastructure.Data.IntegrationTests.Repositories.UserRepositoryTests
{
	public class DeleteUserByIdShould
	{
		[Fact]
		public async void DeleteUserWithExpectedValues_WhenCalledAndSaved()
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
				
				await sut.DeleteUserById(createdUser.UserId);
				await context.UnitOfWork.SaveChangesAsync();

				var actual = await sut.GetUserById(user.UserId);

				// Assert
				actual.Should().BeNull();
			}
		}
	}
}
