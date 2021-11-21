using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using UserRegistration.Core.Interfaces.Persistence;
using UserRegistration.Infrastructure.Data.Entities;
using UserRegistration.Infrastructure.Data.Persistence;

namespace UserRegistration.Infrastructure.Data.DataProviders
{
	internal static class SeedDataProvider
	{
		internal static void SeedTestData(IServiceProvider serviceProvider)
		{
			using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
			using (var context = serviceScope.ServiceProvider.GetService<UserRegistrationDbContext>())
			{
				var unitOfWork = serviceScope.ServiceProvider.GetService<IUnitOfWork>();
				SeedUsers(context);
				unitOfWork.SaveChanges();
			}
		}

		private static void SeedUsers(UserRegistrationDbContext context)
		{
			List<User> users = new List<User>();
			for (int i = 1; i <= 5; i++)
			{
				users.Add(new User
				{
					UserName = $"name{i:000000}",
				});
			}

			context.Users.AddRange(users);
		}
	}
}
