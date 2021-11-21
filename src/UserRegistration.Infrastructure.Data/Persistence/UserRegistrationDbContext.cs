using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserRegistration.Infrastructure.Data.Entities;

namespace UserRegistration.Infrastructure.Data.Persistence
{
	public class UserRegistrationDbContext : DbContext
	{

		public UserRegistrationDbContext(DbContextOptions<UserRegistrationDbContext> options) : base(options) { }

		public DbSet<User> Users { get; set; }

		private Type CallingType => new System.Diagnostics.StackTrace()
			.GetFrame(2)
			.GetMethod()
			.DeclaringType;

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserRegistrationDbContext).Assembly);
			base.OnModelCreating(modelBuilder);
		}

		public override int SaveChanges()
		{
			throw new InvalidOperationException(DataResources.SaveChanges_NotAllowed);
		}

		public override int SaveChanges(bool acceptAllChangesOnSuccess)
		{
			if (CallingType != typeof(UnitOfWork))
			{
				throw new InvalidOperationException(DataResources.SaveChanges_NotAllowed);
			}

			return base.SaveChanges(acceptAllChangesOnSuccess);
		}

		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
		{
			throw new InvalidOperationException(DataResources.SaveChanges_NotAllowed);
		}

		public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
		{
			if (CallingType != typeof(UnitOfWork))
			{
				throw new InvalidOperationException(DataResources.SaveChanges_NotAllowed);
			}

			return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
		}
    }
}
