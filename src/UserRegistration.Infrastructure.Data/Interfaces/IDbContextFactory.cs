using Microsoft.EntityFrameworkCore;

namespace UserRegistration.Infrastructure.Data.Interfaces
{
	public interface IDbContextFactory<TContext> where TContext : DbContext
	{
		TContext CreateDbContext();
	}
}
