using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using UserRegistration.Infrastructure.Data.DataProviders;

namespace UserRegistration.Infrastructure.Data
{
	public static class ApplicationBuilderExtensions
	{
	
		public static IApplicationBuilder SeedTestData(this IApplicationBuilder builder)
		{
			using var serviceScope = builder.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
			SeedDataProvider.SeedTestData(serviceScope.ServiceProvider);

			return builder;
		}
	}
}
