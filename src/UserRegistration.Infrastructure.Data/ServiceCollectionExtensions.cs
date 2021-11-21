using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NetCore.AutoRegisterDi;
using UserRegistration.Core.Interfaces.Persistence;
using UserRegistration.Infrastructure.Data.Persistence;

namespace UserRegistration.Infrastructure.Data
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddInMemoryDatabase(this IServiceCollection services)
		{
			return services
				.AddDbContext<UserRegistrationDbContext>(options => options
					.UseInMemoryDatabase(nameof(UserRegistrationDbContext)));
		}

		public static IServiceCollection AddRepositories(this IServiceCollection services)
		{
			services.AddScoped<IUnitOfWork, UnitOfWork>()
				.AddScoped<IDbContextFactory<UserRegistrationDbContext>, DbContextFactory<UserRegistrationDbContext>>()
				.RegisterAssemblyPublicNonGenericClasses()
				.Where(s => s.Assembly == typeof(UserRegistrationDbContext).Assembly &&
				            s.Name.EndsWith("Repository"))
				.AsPublicImplementedInterfaces(ServiceLifetime.Scoped);

			return services;
		}
	}
}
