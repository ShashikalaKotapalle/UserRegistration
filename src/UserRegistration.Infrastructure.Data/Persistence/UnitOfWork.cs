using System.Threading.Tasks;
using UserRegistration.Core.Interfaces.Persistence;

namespace UserRegistration.Infrastructure.Data.Persistence
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly UserRegistrationDbContext _dbContext;

		public UnitOfWork(UserRegistrationDbContext context)
		{
			_dbContext = context;
		}

		public void SaveChanges()
		{
			_dbContext.SaveChanges(true);
		}

		public Task SaveChangesAsync()
		{
			return _dbContext.SaveChangesAsync(true);
		}

	}
}
