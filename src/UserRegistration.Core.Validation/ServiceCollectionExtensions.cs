using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using NetCore.AutoRegisterDi;
using UserRegistration.Core.Interfaces.Services.Validation;
using UserRegistration.Core.Services.Validation;

namespace UserRegistration.Core.Validation
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddCoreValidators(this IServiceCollection services)
		{
			services.AddTransient(typeof(IValidationService), typeof(ValidationService));

			services.RegisterAssemblyPublicNonGenericClasses(typeof(Validation.ServiceCollectionExtensions).Assembly)
				.Where(c => c.Name.EndsWith("Validator"))
				.AsPublicImplementedInterfaces();

			return services;
		}
	}
}
