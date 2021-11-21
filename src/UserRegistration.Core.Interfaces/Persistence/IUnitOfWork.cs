using System.Threading.Tasks;

namespace UserRegistration.Core.Interfaces.Persistence
{
	public interface IUnitOfWork
	{
		void SaveChanges();
		Task SaveChangesAsync();
	}
}
