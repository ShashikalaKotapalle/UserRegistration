using Microsoft.Extensions.DependencyInjection;
using NetCore.AutoRegisterDi;
using UserRegistration.Core.Interfaces.Services.Validation;
using UserRegistration.Core.Services.Validation;

namespace UserRegistration.Core.Services
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddCoreServices(this IServiceCollection services)
		{
			services.RegisterAssemblyPublicNonGenericClasses(typeof(ServiceCollectionExtensions).Assembly)
				.Where(s => s.Name.EndsWith("Service"))
				.AsPublicImplementedInterfaces(ServiceLifetime.Scoped);

			return services;
		}
	}
}
