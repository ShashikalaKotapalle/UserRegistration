using System;
using AutoFixture;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using UserRegistration.Core.Interfaces.Persistence;
using UserRegistration.Infrastructure.Data.Persistence;
using UserRegistration.Infrastructure.Data.Repositories;

namespace UserRegistration.Infrastructure.Data.IntegrationTests.Repositories.UserRepositoryTests
{
	public class UserRepositoryTestContext : IDisposable
	{
		private readonly IServiceScope _serviceScope;

		public UserRepositoryTestContext()
		{
			_serviceScope = new ServiceCollection()
				.AddInMemoryDatabase()
				.AddScoped<IUnitOfWork, UnitOfWork>()
				.AddScoped<UserRepository>()
				.AddAutoMapper(typeof(UserRegistrationDbContext).Assembly)
				.BuildServiceProvider()
				.CreateScope();

			Sut = _serviceScope.ServiceProvider.GetService<UserRepository>();
			UnitOfWork = _serviceScope.ServiceProvider.GetService<IUnitOfWork>();
			UserRegistrationDbContext = _serviceScope.ServiceProvider.GetService<UserRegistrationDbContext>();
			UserRegistrationDbContext.Database.EnsureCreated();
			Mapper = _serviceScope.ServiceProvider.GetService<IMapper>();
			Fixture = new Fixture();
		}

		public UserRepository Sut { get; set; }
		public UserRegistrationDbContext UserRegistrationDbContext { get; set; }
		public IUnitOfWork UnitOfWork { get; set; }
		public Fixture Fixture { get; set; }
		public IMapper Mapper { get; set; }

		public void Dispose()
		{
			_serviceScope?.Dispose();
			UserRegistrationDbContext?.Dispose();
		}
	}
}
