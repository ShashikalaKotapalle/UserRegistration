using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using UserRegistration.Core.Interfaces.Persistence;
using UserRegistration.Infrastructure.Data.Persistence;
using Xunit;

namespace UserRegistration.Infrastructure.Data.IntegrationTests.UserRegistrationDbContextTests
{
	public class SaveChangesShould
	{
		[Fact]
		public void ThrowNotImplementedException_WhenCalledDirectlyFromDbContextWithoutArguments()
		{
			// Arrange
			var services = new ServiceCollection()
				.AddInMemoryDatabase()
				.AddRepositories()
				.BuildServiceProvider();

			// Act
			var sut = services.GetService<UserRegistrationDbContext>();
			var expected = new InvalidOperationException(DataResources.SaveChanges_NotAllowed);
			var actual = Record.Exception(() => sut.SaveChanges());

			// Assert
			actual.Should()
				.BeOfType<InvalidOperationException>()
				.And
				.BeEquivalentTo(expected, ex => ex
					.Including(e => e.Message));
		}

		[Fact]
		public void ThrowNotImplementedException_WhenCalledDirectlyFromDbContextWithArguments()
		{
			// Arrange
			var services = new ServiceCollection()
				.AddInMemoryDatabase()
				.AddRepositories()
				.BuildServiceProvider();

			// Act
			var sut = services.GetService<UserRegistrationDbContext>();
			var expected = new InvalidOperationException(DataResources.SaveChanges_NotAllowed);
			var actual = Record.Exception(() => sut.SaveChanges(true));

			// Assert
			actual.Should()
				.BeOfType<InvalidOperationException>()
				.And
				.BeEquivalentTo(expected, ex => ex
					.Including(e => e.Message));
		}

		[Fact]
		public void NotThrowException_WhenCalledViaUnitOfWork()
		{
			// Arrange
			var services = new ServiceCollection()
				.AddInMemoryDatabase()
				.AddRepositories()
				.BuildServiceProvider();

			// Act
			var sut = services.GetService<IUnitOfWork>();
			var actual = Record.Exception(() => sut.SaveChanges());

			// Assert
			actual.Should()
				.BeNull();
		}
	}
}
